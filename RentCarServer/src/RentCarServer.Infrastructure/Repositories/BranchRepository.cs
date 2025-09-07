using GenericRepository;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class BranchRepository : Repository<Branch, ApplicationDbContext>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
