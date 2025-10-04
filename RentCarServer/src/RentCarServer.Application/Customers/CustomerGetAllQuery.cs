using RentCarServer.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;

namespace RentCarServer.Application.Customers
{
    public sealed record CustomerGetAllQuery : IRequest<IQueryable<CustomerDto>>;

    internal sealed class CustomerGetAllQueryHandler(
        ICustomerRepository repository) : IRequestHandler<CustomerGetAllQuery, IQueryable<CustomerDto>>
    {
        public Task<IQueryable<CustomerDto>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken) =>
            Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
    }
}
