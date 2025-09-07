using RentCarServer.Application.Branches;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Roles
{
    public sealed class RoleDto : EntityDto
    {
        public string Name { get; set; } = default!;
        public int PermissionCount { get; set; }
        public List<string> Permissions { get; set; } = new();
    }

    public static class RoleExtensions
    {
        public static IQueryable<RoleDto> MapTo(this IQueryable<Role> entity, IQueryable<User> users)
        {
            var res = entity
               .ApplyAuditDto(users)
               .Select(s => new RoleDto
               {
                   Id = s.Entity.Id,
                   Name = s.Entity.Name,
                   PermissionCount = s.Entity.Permissions.Count,
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

        public static IQueryable<RoleDto> MapToGet(this IQueryable<Role> entity, IQueryable<User> users)
        {
            var res = entity
               .ApplyAuditDto(users)
               .Select(s => new RoleDto
               {
                   Id = s.Entity.Id,
                   Name = s.Entity.Name,
                   PermissionCount = s.Entity.Permissions.Count,
                   Permissions = s.Entity.Permissions.Select(s => s.Value).ToList(),
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
