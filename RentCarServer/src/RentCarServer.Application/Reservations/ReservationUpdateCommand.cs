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
    [Permission("reservation:update")]
    public sealed record ReservationUpdateCommand(
    Guid Id,
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
    decimal Total
) : IRequest<Result<string>>;

    public sealed class ReservationUpdateCommandValidator : AbstractValidator<ReservationUpdateCommand>
    {
        public ReservationUpdateCommandValidator()
        {
            RuleFor(x => x.PickUpDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Teslim alma tarihi bugünden önce olamaz.");

            RuleFor(x => x.DeliveryDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Teslim etme tarihi bugünden önce olamaz.");
        }
    }

    internal sealed class ReservationUpdateCommandHandler(
        IBranchRepository branchRepository,
        ICustomerRepository customerRepository,
        IReservationRepository reservationRepository,
        IVehicleRepository vehicleRepository,
        IClaimContext claimContext,
        IUnitOfWork unitOfWork) : IRequestHandler<ReservationUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ReservationUpdateCommand request, CancellationToken cancellationToken)
        {
            Reservation? reservation = await reservationRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (reservation is null)
            {
                return Result<string>.Failure("Rezervasyon bulunamadı");
            }

            if (reservation.Status == Status.Completed || reservation.Status == Status.Canceled)
            {
                return Result<string>.Failure("Bu rezervasyon değiştirilemez");
            }

            var locationId = request.PickUpLocationId ?? claimContext.GetBranchId();

            #region Şube, Müşteri ve Araç Kontrolü
            if (reservation.PickUpLocationId != locationId)
            {
                var isBranchExists = await branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
                if (!isBranchExists)
                {
                    return Result<string>.Failure("Şube bulunamadı");
                }
            }

            if (reservation.CustomerId != request.CustomerId)
            {
                var isCustomerExists = await customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
                if (!isCustomerExists)
                {
                    return Result<string>.Failure("Müşteri bulunamadı");
                }
            }

            if (reservation.VehicleId != request.VehicleId)
            {
                var isVehicleExists = await vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
                if (!isVehicleExists)
                {
                    return Result<string>.Failure("Araç bulunamadı");
                }
            }
            #endregion

            #region Araç Müsaitlik Kontrolü
            if (reservation.PickUpDate != request.PickUpDate
                || reservation.PickUpTime != request.PickUpTime
                | reservation.DeliveryDate != request.DeliveryDate
                || reservation.DeliveryTime != request.DeliveryTime
                )
            {
                // Yeni rezervasyonun alınma ve teslim datetime’ı
                var requestedPickUp = request.PickUpDate.ToDateTime(request.PickUpTime);
                var requestedDelivery = request.DeliveryDate.ToDateTime(request.DeliveryTime);

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
            }
            #endregion

            #region Reservation Objesinin Oluşturulması
            IEnumerable<ReservationExtra> reservationExtras = request.ReservationExtras.Select(s => new ReservationExtra(s.ExtraId, s.Price));

            reservation.SetCustomerId(request.CustomerId);
            reservation.SetPickUpLocationId(locationId);
            reservation.SetPickUpDate(request.PickUpDate);
            reservation.SetPickUpTime(request.PickUpTime);
            reservation.SetDeliveryDate(request.DeliveryDate);
            reservation.SetDeliveryTime(request.DeliveryTime);
            reservation.SetVehicleId(request.VehicleId);
            reservation.SetVehicleDailyPrice(request.VehicleDailyPrice);
            reservation.SetProtectionPackageId(request.ProtectionPackageId);
            reservation.SetProtectionPackagePrice(request.ProtectionPackagePrice);
            reservation.SetReservationExtras(reservationExtras);
            reservation.SetNote(request.Note);
            reservation.SetTotal(request.Total);
            #endregion

            reservationRepository.Update(reservation);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Rezervasyon başarıyla güncellendi";
        }
    }
}
