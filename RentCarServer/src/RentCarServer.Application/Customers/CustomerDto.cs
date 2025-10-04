using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Customers
{
    public sealed class CustomerDto : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string IdentityNumber { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateOnly DrivingLicenseIssuanceDate { get; set; }
        public string FullAddress { get; set; } = default!;
    }

    public static class CustomerExtensions
    {
        public static IQueryable<CustomerDto> MapTo(this IQueryable<EntityWithAuditDto<Customer>> entities)
        {
            return entities.Select(s => new CustomerDto
            {
                Id = s.Entity.Id,
                FirstName = s.Entity.FirstName,
                LastName = s.Entity.LastName,
                FullName = s.Entity.FullName,
                IdentityNumber = s.Entity.IdentityNumber,
                DateOfBirth = s.Entity.DateOfBirth,
                PhoneNumber = s.Entity.PhoneNumber,
                Email = s.Entity.Email,
                DrivingLicenseIssuanceDate = s.Entity.DrivingLicenseIssuanceDate,
                FullAddress = s.Entity.FullAddress,
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
