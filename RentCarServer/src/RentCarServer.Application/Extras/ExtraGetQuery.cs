using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Extras
{
    [Permission("extra:view")]
    public sealed record ExtraGetQuery(Guid Id) : IRequest<Result<ExtraDto>>;
    
    internal sealed class ExtraGetQueryHandler(
        IExtraRepository repository) : IRequestHandler<ExtraGetQuery, Result<ExtraDto>>
    {
        public async Task<Result<ExtraDto>> Handle(ExtraGetQuery request, CancellationToken cancellationToken)
        {
            var res = await repository
                .GetAllWithAudit()
                .MapTo()
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
                return Result<ExtraDto>.Failure("Ekstra bulunamadý");

            return res;
        }
    }
}
