using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Customers
{
    public interface ICustomerRepository : IAuditableRepository<Customer>
    {
    }
}
