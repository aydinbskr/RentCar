using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;

namespace RentCarServer.Application.ProtectionPackages
{
    [Permission("protectionPackage:view")]
    public sealed record ProtectionPackageGetQuery(Guid Id) : IRequest<Result<ProtectionPackageDto>>;

    internal sealed class ProtectionPackageGetQueryHandler(
        IProtectionPackageRepository protectionPackageRepository) : IRequestHandler<ProtectionPackageGetQuery, Result<ProtectionPackageDto>>
    {
        public async Task<Result<ProtectionPackageDto>> Handle(ProtectionPackageGetQuery request, CancellationToken cancellationToken)
        {
            var res = await protectionPackageRepository
                .GetAllWithAudit()
                .MapToGet()
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
            {
                return Result<ProtectionPackageDto>.Failure("Koruma paketi bulunamadý");
            }

            return res;
        }
    }
}