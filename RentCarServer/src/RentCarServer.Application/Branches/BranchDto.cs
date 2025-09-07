using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Branches
{
    public sealed class BranchDto : EntityDto
    {
        public string Name { get; set; } = default!;
        public Address Address { get; set; } = default!;

        public Contact Contact { get; set; } = default!;
    }

    public static class BranchExtensions
    {
        public static IQueryable<BranchDto> MapTo(this IQueryable<Branch> entity, IQueryable<User> users)
        {
            var res = entity
                .ApplyAuditDto(users)
                .Select(s => new BranchDto
                {
                    Id = s.Entity.Id,
                    Name = s.Entity.Name,
                    Address = s.Entity.Address,
                    Contact = s.Entity.Contact,
                    CreatedAt = s.Entity.CreatedAt,
                    CreatedBy = s.Entity.CreatedBy,
                    IsActive = s.Entity.IsActive,
                    UpdatedAt = s.Entity.UpdatedAt,
                    UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,
                    CreatedFullName = s.CreatedUser.FullName,
                    UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName
                })
                .AsQueryable();

            return res;
        }
    }
}
