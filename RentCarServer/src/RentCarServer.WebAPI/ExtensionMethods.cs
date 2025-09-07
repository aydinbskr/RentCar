using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Users;

namespace RentCarServer.WebAPI
{
    public static class ExtensionMethods
    {
        public static async Task CreateFirstUser(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
            var roleRepository = scoped.ServiceProvider.GetRequiredService<IRoleRepository>();
            var branchRepository = scoped.ServiceProvider.GetRequiredService<IBranchRepository>();
            var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

            Branch? branch = await branchRepository.FirstOrDefaultAsync(i => i.Name == "Merkez Şube");
            Role? role = await roleRepository.FirstOrDefaultAsync(i => i.Name == "sys_admin");

            if (branch is null)
            {
                Address address = new(
                    "Kayseri",
                    "KOCASİNAN",
                    "Kayseri merkez");
                Contact contact = new(
                    "3522251015",
                    "3522251016",
                    "info@rentcar.com");
                branch = new("Merkez Şube", address, contact, true);
                branchRepository.Add(branch);
            }

            if (role is null)
            {
                role = new("sys_admin", true);
                roleRepository.Add(role);
            }

            if (!(await userRepository.AnyAsync(p => p.UserName == "admin")))
            {
                string firstName ="Aydın";
                string lastName = "Başkara";
                string email = "aydinbskr@gmail.com";
                string userName = "admin";
                Password password = new("1");
                Guid branchId = branch.Id;
                Guid roleId = role.Id;

                var user = new User(
                    firstName,
                    lastName,
                    email,
                    userName,
                    password,
                    branchId,
                    roleId,
                    true);

                userRepository.Add(user);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public static async Task CleanRemovedPermissionsFromRoleAsync(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var srv = scoped.ServiceProvider;
            var service = srv.GetRequiredService<PermissionCleanerSevice>();
            await service.CleanRemovedPermissionsFromRolesAsync();
        }
    }
}
