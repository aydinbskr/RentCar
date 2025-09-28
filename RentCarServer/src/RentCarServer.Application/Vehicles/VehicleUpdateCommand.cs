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
    [Permission("vehicle:update")]
    public sealed record VehicleUpdateCommand(
    Guid Id,
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
    IFormFile? File,
    bool IsActive
) : IRequest<Result<string>>;

    public sealed class VehicleUpdateCommandValidator : AbstractValidator<VehicleUpdateCommand>
    {
        public VehicleUpdateCommandValidator()
        {
            RuleFor(p => p.Brand)
            .NotEmpty()
            .WithMessage("Marka alanı boş bırakılamaz.");

            RuleFor(p => p.Model)
                .NotEmpty()
                .WithMessage("Model alanı boş bırakılamaz.");

            RuleFor(p => p.ModelYear)
                .GreaterThan(1900)
                .WithMessage("Geçerli bir model yılı seçmelisiniz.");

            RuleFor(p => p.Plate)
                .NotEmpty()
                .WithMessage("Plaka bilgisi girilmelidir.");

            RuleFor(p => p.Features)
                .Must(i => i != null && i.Any())
                .WithMessage("En az bir özellik seçmelisiniz.");
        }
    }

    internal sealed class VehicleUpdateCommandHandler(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<VehicleUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(VehicleUpdateCommand request, CancellationToken cancellationToken)
        {
            Vehicle? vehicle = await vehicleRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (vehicle is null)
                return Result<string>.Failure("Araç bulunamadý");

            if (!string.Equals(vehicle.Plate, request.Plate, StringComparison.OrdinalIgnoreCase))
            {
                bool plateExists = await vehicleRepository.AnyAsync(
                    p => p.Plate == request.Plate && p.Id != request.Id, cancellationToken);
                if (plateExists)
                    return Result<string>.Failure("Bu plaka ile kayıtlı başka bir araç var.");
            }


            string imageUrl = vehicle.ImageUrl;
            if (request.File is not null && request.File.Length > 0)
            {
                imageUrl = FileService.FileSaveToServer(request.File, "wwwroot/images/");
            }

            vehicle.SetBrand(request.Brand);
            vehicle.SetModel(request.Model);
            vehicle.SetModelYear(request.ModelYear);
            vehicle.SetColor(request.Color);
            vehicle.SetPlate(request.Plate);
            vehicle.SetCategoryId(request.CategoryId);
            vehicle.SetBranchId(request.BranchId);
            vehicle.SetVinNumber(request.VinNumber);
            vehicle.SetEngineNumber(request.EngineNumber);
            vehicle.SetDescription(request.Description);
            vehicle.SetImageUrl(imageUrl);
            vehicle.SetFuelType(request.FuelType);
            vehicle.SetTransmission(request.Transmission);
            vehicle.SetEngineVolume(request.EngineVolume);
            vehicle.SetEnginePower(request.EnginePower);
            vehicle.SetTractionType(request.TractionType);
            vehicle.SetFuelConsumption(request.FuelConsumption);
            vehicle.SetSeatCount(request.SeatCount);
            vehicle.SetKilometer(request.Kilometer);
            vehicle.SetDailyPrice(request.DailyPrice);
            vehicle.SetWeeklyDiscountRate(request.WeeklyDiscountRate);
            vehicle.SetMonthlyDiscountRate(request.MonthlyDiscountRate);
            vehicle.SetInsuranceType(request.InsuranceType);
            vehicle.SetLastMaintenanceDate(request.LastMaintenanceDate);
            vehicle.SetLastMaintenanceKm(request.LastMaintenanceKm);
            vehicle.SetNextMaintenanceKm(request.NextMaintenanceKm);
            vehicle.SetInspectionDate(request.InspectionDate);
            vehicle.SetInsuranceEndDate(request.InsuranceEndDate);
            if (request.CascoEndDate is not null)
            {
                vehicle.SetCascoEndDate(request.CascoEndDate);
            }
            vehicle.SetTireStatus(request.TireStatus);
            vehicle.SetGeneralStatus(request.GeneralStatus);
            vehicle.SetFeatures(request.Features.Select(f => new Feature(f)));
            vehicle.SetStatus(request.IsActive);

            vehicleRepository.Update(vehicle);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Araç başarıyla güncellendi";
        }
    }
}
