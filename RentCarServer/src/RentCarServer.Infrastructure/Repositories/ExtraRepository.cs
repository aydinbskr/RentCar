using RentCarServer.Domain.Extras;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class ExtraRepository : AuditableRepository<Extra, ApplicationDbContext>, IExtraRepository
    {
        public ExtraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
