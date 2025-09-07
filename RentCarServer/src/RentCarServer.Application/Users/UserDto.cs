using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Users
{
    public sealed class UserDto : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = default!;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
    }

    public static class UserExtensions
    {
        public static IQueryable<UserDto> MapTo(
            this IQueryable<EntityWithAuditDto<User>> entities,
            IQueryable<Role> roles,
            IQueryable<Branch> branches
            )
        {
            var res = entities
                .Join(roles, m => m.Entity.RoleId, m => m.Id, (e, role)
                    => new { e.Entity, e.CreatedUser, e.UpdatedUser, Role = role })
                .Join(branches, m => m.Entity.BranchId, m => m.Id, (entity, branch)
                    => new { entity.Entity, entity.CreatedUser, entity.UpdatedUser, entity.Role, Branch = branch })
                .Select(s => new UserDto
                {
                    Id = s.Entity.Id,
                    FirstName = s.Entity.FirstName,
                    LastName = s.Entity.LastName,
                    FullName = s.Entity.FullName,
                    Email = s.Entity.Email,
                    UserName = s.Entity.UserName,
                    RoleId = s.Entity.RoleId,
                    RoleName = s.Role.Name,
                    BranchId = s.Entity.BranchId,
                    BranchName = s.Branch.Name,
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
