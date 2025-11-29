using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations
{
    [Permission("reservation:create")]
    public sealed record CreditCartInformation(
    string CartNumber,
    string Owner,
    string Expiry,
    string CCV);

    public sealed record ReservationCreateCommand(
        Guid CustomerId,
        Guid? PickUpLocationId,
        DateOnly PickUpDate,
        TimeOnly PickUpTime,
        DateOnly DeliveryDate,
        TimeOnly DeliveryTime,
        Guid VehicleId,
        decimal VehicleDailyPrice,
        Guid ProtectionPackageId,
        decimal ProtectionPackagePrice,
        List<ReservationExtra> ReservationExtras,
        string Note,
        CreditCartInformation CreditCartInformation,
        decimal Total
    ) : IRequest<Result<string>>;

    public sealed class ReservationCreateCommandValidator : AbstractValidator<ReservationCreateCommand>
    {
        public ReservationCreateCommandValidator()
        {
            RuleFor(x => x.CreditCartInformation.CartNumber)
                .NotEmpty()
                .WithMessage("Kart numarası boş bırakılamaz.");

            RuleFor(x => x.CreditCartInformation.Owner)
                .NotEmpty()
                .WithMessage("Kart sahibi adı boş bırakılamaz.");

            RuleFor(x => x.CreditCartInformation.Expiry)
               .NotEmpty()
               .WithMessage("Son kullanma tarihi boş bırakılamaz.");

            RuleFor(x => x.CreditCartInformation.CCV)
               .NotEmpty()
               .WithMessage("CCV boş bırakılamaz.");

            RuleFor(x => x.PickUpDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Teslim alma tarihi bugünden önce olamaz.");

            RuleFor(x => x.DeliveryDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Teslim etme tarihi bugünden önce olamaz.");
        }
    }


    internal sealed class ReservationCreateCommandHandler(
        IBranchRepository branchRepository,
        ICustomerRepository customerRepository,
        IReservationRepository reservationRepository,
        IVehicleRepository vehicleRepository,
        IClaimContext claimContext,
        IUnitOfWork unitOfWork) : IRequestHandler<ReservationCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ReservationCreateCommand request, CancellationToken cancellationToken)
        {
            var locationId = request.PickUpLocationId ?? claimContext.GetBranchId();

            #region Şube, Müşteri ve Araç Kontrolü
            var isBranchExists = await branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
            if (!isBranchExists)
            {
                return Result<string>.Failure("Şube bulunamadı");
            }

            var isCustomerExists = await customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
            if (!isCustomerExists)
            {
                return Result<string>.Failure("Müşteri bulunamadı");
            }

            var isVehicleExists = await vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
            if (!isVehicleExists)
            {
                return Result<string>.Failure("Araç bulunamadı");
            }
            #endregion

            #region Araç Müsaitlik Kontrolü
            // Yeni rezervasyonun alınma ve teslim datetime’ı
            var requestedPickUp = request.PickUpDate.ToDateTime(request.PickUpTime);
            var requestedDelivery = request.DeliveryDate.ToDateTime(request.DeliveryTime);

            // Aynı araç için bu zaman aralığında çakışan rezervasyon var mı kontrol et
            var possibleOverlaps = await reservationRepository
                .Where(r => r.VehicleId == request.VehicleId
                && (r.Status.Value == Status.Pending.Value || r.Status.Value == Status.Delivered.Value))
            .Select(s => new
            {
                Id = s.Id,
                VehicleId = s.VehicleId,
                DeliveryDate = s.DeliveryDate,
                DeliveryTime = s.DeliveryTime,
                PickUpDate = s.PickUpDate,
                PickUpTime = s.PickUpTime,
            })
            .ToListAsync(cancellationToken);

            var overlaps = possibleOverlaps.Any(r =>
                requestedPickUp < r.DeliveryDate.ToDateTime(r.DeliveryTime).AddHours(1) &&
                requestedDelivery > r.PickUpDate.ToDateTime(r.PickUpTime)
            );


            if (overlaps)
            {
                return Result<string>.Failure("Seçilen araç, belirtilen tarih ve saat aralığında müsait değil.");
            }
            #endregion

            #region Ödeme İşlemi
            // ödeme işlemi yapıp başarılı ise ona göre devam etmeliyiz
            #endregion

            #region Reservation Objesinin Oluşturulması

            IEnumerable<ReservationExtra> reservationExtras = request.ReservationExtras.Select(s => new ReservationExtra(s.ExtraId, s.Price));

            var last4Digits = request.CreditCartInformation.CartNumber[^4..];
            PaymentInformation paymentInformation = new(last4Digits, request.CreditCartInformation.Owner);
            Status status = Status.Pending;

            Reservation reservation = Reservation.Create(
                request.CustomerId,
                locationId,
                request.PickUpDate,
                request.PickUpTime,
                request.DeliveryDate,
                request.DeliveryTime,
                request.VehicleId,
                request.VehicleDailyPrice,
                request.ProtectionPackageId,
                request.ProtectionPackagePrice,
                reservationExtras,
                request.Note,
                paymentInformation,
                status,
                request.Total,
                0,
                new ReservationHistory(
                    "",
                    "Rezervasyon oluşturuldu",
                    DateTime.Now
                )
            );
            #endregion

            reservationRepository.Add(reservation);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Rezervasyon başarıyla oluşturuldu";
        }
    }
}
