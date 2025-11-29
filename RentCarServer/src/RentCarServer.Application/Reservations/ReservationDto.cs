using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Reservations
{
    public sealed class ReservationPickUpDto
    {
        public string Name { get; set; } = default!;
        public string FullAddress { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
    public sealed class ReservationCustomerDto
    {
        public string FullName { get; set; } = default!;
        public string IdentityNumber { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FullAddress { get; set; } = default!;
    }
    public sealed class ReservationVehicleDto
    {
        public Guid Id { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public string Model { get; set; } = default!;
        public int ModelYear { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal FuelConsumption { get; set; } = default!;
        public int SeatCount { get; set; } = default!;
        public string TractionType { get; set; } = default!;
        public int Kilometer { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string Plate { get; set; } = default!;
    }
    public sealed class ReservationExtraDto
    {
        public Guid ExtraId { get; set; }
        public string ExtraName { get; set; } = default!;
        public decimal Price { get; set; }
    }
    public sealed class ReservationDto : EntityDto
    {
        public string ReservationNumber { get; set; } = default!;
        public Guid CustomerId { get; set; } = default!;
        public ReservationCustomerDto Customer { get; set; } = default!;
        public Guid PickUpLocationId { get; set; } = default!;
        public ReservationPickUpDto PickUp { get; set; } = default!;
        public DateOnly PickUpDate { get; set; } = default!;
        public TimeOnly PickUpTime { get; set; } = default!;
        public DateTimeOffset PickUpDateTime { get; set; } = default!;
        public DateOnly DeliveryDate { get; set; } = default!;
        public TimeOnly DeliveryTime { get; set; } = default!;
        public DateTimeOffset DeliveryDateTime { get; set; } = default!;
        public Guid VehicleId { get; set; } = default!;
        public decimal VehicleDailyPrice { get; set; } = default!;
        public ReservationVehicleDto Vehicle { get; set; } = default!;
        public Guid ProtectionPackageId { get; set; } = default!;
        public decimal ProtectionPackagePrice { get; set; } = default!;
        public string ProtectionPackageName { get; set; } = default!;
        public List<ReservationExtraDto> ReservationExtras { get; set; } = default!;
        public string Note { get; set; } = default!;
        public decimal Total { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int TotalDay { get; set; } = default!;
        public PaymentInformation PaymentInformation { get; set; } = default!;
        public List<ReservationHistory> Histories { get; set; } = default!;
    }

    public static class ReservationExtensions
    {
        public static IQueryable<ReservationDto> MapTo(
            this IQueryable<EntityWithAuditDto<Reservation>> entities,
                 IQueryable<Customer> customers,
                 IQueryable<Branch> branches,
                 IQueryable<Vehicle> vehicles,
                 IQueryable<Category> categories,
                 IQueryable<ProtectionPackage> protectionPackages,
                 IQueryable<Extra> extras
           )
        {
            var res = entities
                .Join(customers, m => m.Entity.CustomerId, m => m.Id, (r, customer) => new
                {
                    r.Entity,
                    r.CreatedUser,
                    r.UpdatedUser,
                    Customer = customer
                })
                .Join(branches, m => m.Entity.PickUpLocationId, m => m.Id, (r, branch) => new
                {
                    r.Entity,
                    r.CreatedUser,
                    r.UpdatedUser,
                    r.Customer,
                    Branch = branch
                })
                .Join(protectionPackages, m => m.Entity.ProtectionPackageId, m => m.Id, (r, protectionPackage) => new
                {
                    r.Entity,
                    r.CreatedUser,
                    r.UpdatedUser,
                    r.Customer,
                    r.Branch,
                    ProtectionPackage = protectionPackage
                })
                .Join(vehicles, m => m.Entity.VehicleId, m => m.Id, (r, vehicle) => new
                {
                    r.Entity,
                    r.CreatedUser,
                    r.UpdatedUser,
                    r.Customer,
                    r.Branch,
                    r.ProtectionPackage,
                    Vehicle = vehicle
                })
                .Select(s => new ReservationDto
                {
                    Id = s.Entity.Id,
                    ReservationNumber = s.Entity.ReservationNumber,
                    CustomerId = s.Entity.CustomerId,
                    Customer = new ReservationCustomerDto
                    {
                        Email = s.Customer.Email,
                        FullAddress = s.Customer.FullAddress,
                        FullName = s.Customer.FullName,
                        IdentityNumber = s.Customer.IdentityNumber,
                        PhoneNumber = s.Customer.PhoneNumber
                    },
                    PickUpLocationId = s.Entity.PickUpLocationId,
                    PickUp = new ReservationPickUpDto
                    {
                        Name = s.Branch.Name,
                        FullAddress = s.Branch.Address.FullAddress,
                        PhoneNumber = s.Branch.Contact.PhoneNumber1
                    },
                    PickUpDate = s.Entity.PickUpDate,
                    PickUpTime = s.Entity.PickUpTime,
                    PickUpDateTime = s.Entity.PickUpDatetime,
                    DeliveryDate = s.Entity.DeliveryDate,
                    DeliveryTime = s.Entity.DeliveryTime,
                    DeliveryDateTime = s.Entity.DeliveryDatetime,
                    VehicleId = s.Entity.VehicleId,
                    VehicleDailyPrice = s.Entity.VehicleDailyPrice,
                    Vehicle = new ReservationVehicleDto
                    {
                        Id = s.Vehicle.Id,
                        Brand = s.Vehicle.Brand,
                        Model = s.Vehicle.Model,
                        ModelYear = s.Vehicle.ModelYear,
                        CategoryName = categories.First(i => i.Id == s.Vehicle.CategoryId).Name,
                        Color = s.Vehicle.Color,
                        FuelConsumption = s.Vehicle.FuelConsumption,
                        SeatCount = s.Vehicle.SeatCount,
                        TractionType = s.Vehicle.TractionType,
                        Kilometer = s.Vehicle.Kilometer,
                        ImageUrl = s.Vehicle.ImageUrl,
                        Plate = s.Vehicle.Plate
                    },
                    ProtectionPackageId = s.Entity.ProtectionPackageId,
                    ProtectionPackagePrice = s.Entity.ProtectionPackagePrice,
                    ProtectionPackageName = s.ProtectionPackage.Name,
                    ReservationExtras = s.Entity.ReservationExtras.Join(extras, m => m.ExtraId, m => m.Id, (re, extra) => new ReservationExtraDto
                    {
                        ExtraId = re.ExtraId,
                        ExtraName = extra.Name,
                        Price = re.Price
                    }).ToList(),
                    Note = s.Entity.Note,
                    Histories = s.Entity.Histories.ToList(),
                    Total = s.Entity.Total,
                    TotalDay = s.Entity.TotalDay,
                    Status = s.Entity.Status.Value,
                    PaymentInformation = s.Entity.PaymentInformation,
                    IsActive = s.Entity.IsActive,
                    CreatedAt = s.Entity.CreatedAt,
                    CreatedBy = s.Entity.CreatedBy,
                    CreatedFullName = s.CreatedUser.FullName,
                    UpdatedAt = s.Entity.UpdatedAt,
                    UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                    UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName : null,
                });
            return res;
        }
    }
}
