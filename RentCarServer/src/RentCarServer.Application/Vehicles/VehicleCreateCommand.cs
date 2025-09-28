using FluentValidation;
using GenericFileService.Files;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Vehicles
{

    [Permission("vehicle:create")]
    public sealed record VehicleCreateCommand(
        string Brand,
        string Model,
        int ModelYear,
        string Color,
        string Plate,
        Guid CategoryId,
        Guid BranchId,
        string VinNumber,
        string EngineNumber,
        string Description,
        string FuelType,
        string Transmission,
        decimal EngineVolume,
        int EnginePower,
        string TractionType,
        decimal FuelConsumption,
        int SeatCount,
        int Kilometer,
        decimal DailyPrice,
        decimal WeeklyDiscountRate,
        decimal MonthlyDiscountRate,
        string InsuranceType,
        DateOnly LastMaintenanceDate,
        int LastMaintenanceKm,
        int NextMaintenanceKm,
        DateOnly InspectionDate,
        DateOnly InsuranceEndDate,
        DateOnly? CascoEndDate,
        string TireStatus,
        string GeneralStatus,
        List<string> Features,
        IFormFile File,
        bool IsActive
    ) : IRequest<Result<string>>;

    public sealed class VehicleCreateCommandValidator : AbstractValidator<VehicleCreateCommand>
    {
        public VehicleCreateCommandValidator()
        {
            RuleFor(p => p.Brand)
            .NotEmpty()
            .WithMessage("Marka alaný boþ bırakılamaz.");

            RuleFor(p => p.Model)
                .NotEmpty()
                .WithMessage("Model alaný boþ bırakılamaz.");

            RuleFor(p => p.ModelYear)
                .GreaterThan(1900)
                .WithMessage("Geçerli bir model yılı seçmelisiniz.");

            RuleFor(p => p.Plate)
                .NotEmpty()
                .WithMessage("Plaka bilgisi girilmelidir.");

            RuleFor(p => p.File)
                .NotEmpty()
                .WithMessage("Araç görseli yüklemelisiniz.");

            RuleFor(p => p.Features)
                .Must(i => i != null && i.Any())
                .WithMessage("En az bir özellik seçmelisiniz.");
        }
    }

    internal sealed class VehicleCreateCommandHandler(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<VehicleCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(VehicleCreateCommand request, CancellationToken cancellationToken)
        {
            if (await vehicleRepository.AnyAsync(p => p.Plate == request.Plate, cancellationToken))
                return Result<string>.Failure("Bu plaka ile kayıtlı başka bir araç var.");

            string fileName = FileService.FileSaveToServer(request.File, "wwwroot/images/");

            IEnumerable<Feature> features = request.Features.Select(f => new Feature(f));

            Vehicle vehicle = new Vehicle(
                request.Brand,
                request.Model,
                request.ModelYear,
                request.Color,
                request.Plate,
                request.CategoryId,
                request.BranchId,
                request.VinNumber,
                request.EngineNumber,
                request.Description,
                fileName,
                request.FuelType,
                request.Transmission,
                request.EngineVolume,
                request.EnginePower,
                request.TractionType,
                request.FuelConsumption,
                request.SeatCount,
                request.Kilometer,
                request.DailyPrice,
                request.WeeklyDiscountRate,
                request.MonthlyDiscountRate,
                request.InsuranceType,
                request.LastMaintenanceDate,
                request.LastMaintenanceKm,
                request.NextMaintenanceKm,
                request.InspectionDate,
                request.InsuranceEndDate,
                request.CascoEndDate,
                request.TireStatus,
                request.GeneralStatus,
                features,
                request.IsActive
            );

            vehicleRepository.Add(vehicle);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Araç başarıyla kaydedildi";
        }
    }
}
