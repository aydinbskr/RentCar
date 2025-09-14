using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;

namespace RentCarServer.Application.ProtectionPackages
{
    [Permission("protectionPackage:view")]
    public sealed record ProtectionPackageGetAllQuery() : IRequest<IQueryable<ProtectionPackageDto>>;

    internal sealed class ProtectionPackageGetAllQueryHandler(
        IProtectionPackageRepository repository) : IRequestHandler<ProtectionPackageGetAllQuery, IQueryable<ProtectionPackageDto>>
    {
        public Task<IQueryable<ProtectionPackageDto>> Handle(ProtectionPackageGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repository.GetAllWithAudit().MapToGet().AsQueryable());
    }
}