using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Vehicles
{
    public sealed class VehicleDto : EntityDto
    {
        public string Brand { get; set; } = default!;
        public string Model { get; set; } = default!;
        public int ModelYear { get; set; }
        public string Color { get; set; } = default!;
        public string Plate { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public Guid BranchId { get; set; }
        public string VinNumber { get; set; } = default!;
        public string EngineNumber { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string FuelType { get; set; } = default!;
        public string Transmission { get; set; } = default!;
        public decimal EngineVolume { get; set; }
        public int EnginePower { get; set; }
        public string TractionType { get; set; } = default!;
        public decimal FuelConsumption { get; set; }
        public int SeatCount { get; set; }
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyDiscountRate { get; set; }
        public decimal MonthlyDiscountRate { get; set; }
        public string InsuranceType { get; set; } = default!;
        public DateOnly LastMaintenanceDate { get; set; }
        public int LastMaintenanceKm { get; set; }
        public int NextMaintenanceKm { get; set; }
        public DateOnly InspectionDate { get; set; }
        public DateOnly InsuranceEndDate { get; set; }
        public DateOnly? CascoEndDate { get; set; }
        public string TireStatus { get; set; } = default!;
        public string GeneralStatus { get; set; } = default!;
        public List<string> Features { get; set; } = new();
    }

    public static class VehicleExtensions
    {
        public static IQueryable<VehicleDto> MapTo(this IQueryable<EntityWithAuditDto<Vehicle>> entities)
        {
            return entities.Select(s => new VehicleDto
            {
                Id = s.Entity.Id,
                Brand = s.Entity.Brand,
                Model = s.Entity.Model,
                ModelYear = s.Entity.ModelYear,
                Color = s.Entity.Color,
                Plate = s.Entity.Plate,
                CategoryId = s.Entity.CategoryId,
                BranchId = s.Entity.BranchId,
                VinNumber = s.Entity.VinNumber,
                EngineNumber = s.Entity.EngineNumber,
                Description = s.Entity.Description,
                ImageUrl = s.Entity.ImageUrl,
                FuelType = s.Entity.FuelType,
                Transmission = s.Entity.Transmission,
                EngineVolume = s.Entity.EngineVolume,
                EnginePower = s.Entity.EnginePower,
                TractionType = s.Entity.TractionType,
                FuelConsumption = s.Entity.FuelConsumption,
                SeatCount = s.Entity.SeatCount,
                Kilometer = s.Entity.Kilometer,
                DailyPrice = s.Entity.DailyPrice,
                WeeklyDiscountRate = s.Entity.WeeklyDiscountRate,
                MonthlyDiscountRate = s.Entity.MonthlyDiscountRate,
                InsuranceType = s.Entity.InsuranceType,
                LastMaintenanceDate = s.Entity.LastMaintenanceDate,
                LastMaintenanceKm = s.Entity.LastMaintenanceKm,
                NextMaintenanceKm = s.Entity.NextMaintenanceKm,
                InspectionDate = s.Entity.InspectionDate,
                InsuranceEndDate = s.Entity.InsuranceEndDate,
                CascoEndDate = s.Entity.CascoEndDate != null ? s.Entity.CascoEndDate.Value : null,
                TireStatus = s.Entity.TireStatus,
                GeneralStatus = s.Entity.GeneralStatus,
                Features = s.Entity.Features.Select(f => f.Value).ToList(),
                IsActive = s.Entity.IsActive,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy,
                CreatedFullName = s.CreatedUser.FullName,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName : null,
            });
        }
    }
}
