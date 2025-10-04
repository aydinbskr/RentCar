using RentCarServer.Domain.Customers;
using RentCarServer.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Repositories
{
    internal sealed class CustomerRepository : AuditableRepository<Customer, ApplicationDbContext>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
