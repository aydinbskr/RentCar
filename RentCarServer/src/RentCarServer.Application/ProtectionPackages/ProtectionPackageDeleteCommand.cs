using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.ProtectionPackages
{
    [Permission("protectionPackage:delete")]
    public sealed record ProtectionPackageDeleteCommand(
        Guid Id) : IRequest<Result<string>>;

    internal sealed class ProtectionPackageDeleteCommandHandler(
        IProtectionPackageRepository protectionPackageRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProtectionPackageDeleteCommand request, CancellationToken cancellationToken)
        {
            var package = await protectionPackageRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (package is null)
            {
                return Result<string>.Failure("Koruma paketi bulunamadý");
            }

            package.Delete();
            protectionPackageRepository.Update(package);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Koruma paketi baþarýyla silindi";
        }
    }
}