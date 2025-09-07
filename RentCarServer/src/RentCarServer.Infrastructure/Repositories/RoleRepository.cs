using GenericRepository;
using RentCarServer.Domain.Roles;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class RoleRepository : Repository<Role, ApplicationDbContext>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
