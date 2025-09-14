using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;

namespace RentCarServer.Application.Extras
{
    [Permission("extra:view")]
    public sealed record ExtraGetAllQuery : IRequest<IQueryable<ExtraDto>>;
    
    internal sealed class ExtraGetAllQueryHandler(
        IExtraRepository repository) : IRequestHandler<ExtraGetAllQuery, IQueryable<ExtraDto>>
    {
        public Task<IQueryable<ExtraDto>> Handle(ExtraGetAllQuery request, CancellationToken cancellationToken) =>
            Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
    }
}
