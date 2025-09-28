using Microsoft.VisualBasic.FileIO;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Vehicles
{
    public sealed class Vehicle : Entity
    {
        private readonly List<Feature> _features = new();
        private Vehicle() { }

        public Vehicle(
            string brand,
            string model,
            int modelYear,
            string color,
            string plate,
            Guid categoryId,
            Guid branchId,
            string vinNumber,
            string engineNumber,
            string description,
            string imageUrl,
            string fuelType,
            string transmission,
            decimal engineVolume,
            int enginePower,
            string tractionType,
            decimal fuelConsumption,
            int seatCount,
            int kilometer,
            decimal dailyPrice,
            decimal weeklyDiscountRate,
            decimal monthlyDiscountRate,
            string insuranceType,
            DateOnly lastMaintenanceDate,
            int lastMaintenanceKm,
            int nextMaintenanceKm,
            DateOnly inspectionDate,
            DateOnly insuranceEndDate,
            DateOnly? cascoEndDate,
            string tireStatus,
            string generalStatus,
            IEnumerable<Feature> features,
            bool isActive)
        {
            SetBrand(brand);
            SetModel(model);
            SetModelYear(modelYear);
            SetColor(color);
            SetPlate(plate);
            SetCategoryId(categoryId);
            SetBranchId(branchId);
            SetVinNumber(vinNumber);
            SetEngineNumber(engineNumber);
            SetDescription(description);
            SetImageUrl(imageUrl);
            SetFuelType(fuelType);
            SetTransmission(transmission);
            SetEngineVolume(engineVolume);
            SetEnginePower(enginePower);
            SetTractionType(tractionType);
            SetFuelConsumption(fuelConsumption);
            SetSeatCount(seatCount);
            SetKilometer(kilometer);
            SetDailyPrice(dailyPrice);
            SetWeeklyDiscountRate(weeklyDiscountRate);
            SetMonthlyDiscountRate(monthlyDiscountRate);
            SetInsuranceType(insuranceType);
            SetLastMaintenanceDate(lastMaintenanceDate);
            SetLastMaintenanceKm(lastMaintenanceKm);
            SetNextMaintenanceKm(nextMaintenanceKm);
            SetInspectionDate(inspectionDate);
            SetInsuranceEndDate(insuranceEndDate);
            if (cascoEndDate is not null)
            {
                SetCascoEndDate(cascoEndDate);
            }
            SetTireStatus(tireStatus);
            SetGeneralStatus(generalStatus);
            SetFeatures(features);
            SetStatus(isActive);
        }

        public string Brand { get; private set; } = default!;
        public string Model { get; private set; } = default!;
        public int ModelYear { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string Plate { get; private set; } = default!;
        public Guid CategoryId { get; private set; } = default!;
        public Guid BranchId { get; private set; } = default!;
        public string VinNumber { get; private set; } = default!;
        public string EngineNumber { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string ImageUrl { get; private set; } = default!;
        public string FuelType { get; private set; } = default!;
        public string Transmission { get; private set; } = default!;
        public decimal EngineVolume { get; private set; } = default!;
        public int EnginePower { get; private set; } = default!;
        public string TractionType { get; private set; } = default!;
        public decimal FuelConsumption { get; private set; } = default!;
        public int SeatCount { get; private set; } = default!;
        public int Kilometer { get; private set; } = default!;
        public decimal DailyPrice { get; private set; } = default!;
        public decimal WeeklyDiscountRate { get; private set; } = default!;
        public decimal MonthlyDiscountRate { get; private set; } = default!;
        public string InsuranceType { get; private set; } = default!;
        public DateOnly LastMaintenanceDate { get; private set; } = default!;
        public int LastMaintenanceKm { get; private set; } = default!;
        public int NextMaintenanceKm { get; private set; } = default!;
        public DateOnly InspectionDate { get; private set; } = default!;
        public DateOnly InsuranceEndDate { get; private set; } = default!;
        public DateOnly? CascoEndDate { get; private set; }
        public string TireStatus { get; private set; } = default!;
        public string GeneralStatus { get; private set; } = default!;
        public IReadOnlyCollection<Feature> Features => _features;

        #region Behaviors
        public void SetBrand(string brand) => Brand = brand;
        public void SetModel(string model) => Model = model;
        public void SetModelYear(int modelYear) => ModelYear = modelYear;
        public void SetColor(string color) => Color = color;
        public void SetPlate(string plate) => Plate = plate;
        public void SetCategoryId(Guid categoryId) => CategoryId = categoryId;
        public void SetBranchId(Guid branchId) => BranchId = branchId;
        public void SetVinNumber(string vinNumber) => VinNumber = vinNumber;
        public void SetEngineNumber(string engineNumber) => EngineNumber = engineNumber;
        public void SetDescription(string description) => Description = description;
        public void SetImageUrl(string imageUrl) => ImageUrl = imageUrl;
        public void SetFuelType(string fuelType) => FuelType = fuelType;
        public void SetTransmission(string transmission) => Transmission = transmission;
        public void SetEngineVolume(decimal engineVolume) => EngineVolume = engineVolume;
        public void SetEnginePower(int enginePower) => EnginePower = enginePower;
        public void SetTractionType(string tractionType) => TractionType = tractionType;
        public void SetFuelConsumption(decimal fuelConsumption) => FuelConsumption = fuelConsumption;
        public void SetSeatCount(int seatCount) => SeatCount = seatCount;
        public void SetKilometer(int kilometer) => Kilometer = kilometer;
        public void SetDailyPrice(decimal dailyPrice) => DailyPrice = dailyPrice;
        public void SetWeeklyDiscountRate(decimal weeklyDiscountRate) => WeeklyDiscountRate = weeklyDiscountRate;
        public void SetMonthlyDiscountRate(decimal monthlyDiscountRate) => MonthlyDiscountRate = monthlyDiscountRate;
        public void SetInsuranceType(string insuranceType) => InsuranceType = insuranceType;
        public void SetLastMaintenanceDate(DateOnly lastMaintenanceDate) => LastMaintenanceDate = lastMaintenanceDate;
        public void SetLastMaintenanceKm(int lastMaintenanceKm) => LastMaintenanceKm = lastMaintenanceKm;
        public void SetNextMaintenanceKm(int nextMaintenanceKm) => NextMaintenanceKm = nextMaintenanceKm;
        public void SetInspectionDate(DateOnly inspectionDate) => InspectionDate = inspectionDate;
        public void SetInsuranceEndDate(DateOnly insuranceEndDate) => InsuranceEndDate = insuranceEndDate;
        public void SetCascoEndDate(DateOnly? cascoEndDate) => CascoEndDate = cascoEndDate;
        public void SetTireStatus(string tireStatus) => TireStatus = tireStatus;
        public void SetGeneralStatus(string generalStatus) => GeneralStatus = generalStatus;
        public void SetFeatures(IEnumerable<Feature> features)
        {
            _features.Clear();
            _features.AddRange(features);
        }
        #endregion
    }
}
