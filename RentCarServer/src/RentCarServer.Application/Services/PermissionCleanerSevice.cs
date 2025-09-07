using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Services
{
    public sealed class PermissionCleanerSevice(
    IRoleRepository roleRepository,
    PermissionService permissionService,
    IUnitOfWork unitOfWork)
    {
        public async Task CleanRemovedPermissionsFromRolesAsync(CancellationToken cancellationToken = default)
        {
            var currentPermissions = permissionService.GetAll();
            var roles = await roleRepository.GetAllWithTracking().ToListAsync(cancellationToken);
            foreach (var role in roles)
            {
                var currentPermissionsForRole = role.Permissions.Select(s => s.Value).ToList();
                var filteredPermissions = currentPermissionsForRole.Where(p => currentPermissions.Contains(p)).ToList();
                var permissions = filteredPermissions.Select(s => new Permission(s)).ToList();
                role.SetPermissions(permissions);
            }
            roleRepository.UpdateRange(roles);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
