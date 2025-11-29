using RentCarServer.Domain.ProtectionPackages;
using TS.Result;
using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using System.Linq;
using TS.MediatR;
using Microsoft.EntityFrameworkCore;

namespace RentCarServer.Application.ProtectionPackages
{
    [Permission("protectionPackage:edit")]
    public sealed record ProtectionPackageUpdateCommand(
        Guid Id,
        string Name,
        decimal Price,
        bool IsRecommended,
        int OrderNumber,
        IEnumerable<string> Coverages,
        bool IsActive) : IRequest<Result<string>>;

    public sealed class ProtectionPackageUpdateCommandValidator : AbstractValidator<ProtectionPackageUpdateCommand>
    {
        public ProtectionPackageUpdateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir paket adý girin");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Geçerli bir fiyat girin");
        }
    }

    internal sealed class ProtectionPackageUpdateCommandHandler(
        IProtectionPackageRepository protectionPackageRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProtectionPackageUpdateCommand request, CancellationToken cancellationToken)
        {
            var package = await protectionPackageRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (package is null)
            {
                return Result<string>.Failure("Koruma paketi bulunamadý");
            }

            if (!string.Equals(package.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await protectionPackageRepository.AnyAsync(p => p.Name == request.Name && p.Id != request.Id, cancellationToken);
                if (nameExists)
                {
                    return Result<string>.Failure("Koruma paketi adý daha önce tanýmlanmýþ");
                }
            }

            if (package.OrderNumber != request.OrderNumber)
            {
                var packages = await protectionPackageRepository
                    .WhereWithTracking(p => p.Id != package.Id)
                    .OrderBy(i => i.OrderNumber)
                    .ToListAsync(cancellationToken);

                packages.Insert(request.OrderNumber - 1, package);

                foreach (var (item, index) in packages.Select((item, index) => (item, index)))
                {
                    item.SetOrderNumber(index + 1);
                }
            }


            package.SetName(request.Name);
            package.SetPrice(request.Price);
            package.SetIsRecommended(request.IsRecommended);
            
            var coverages = (request.Coverages ?? Enumerable.Empty<string>()).Select(c => new ProtectionCoverage(c));
            package.SetCoverages(coverages);
            package.SetStatus(request.IsActive);

            protectionPackageRepository.Update(package);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Koruma paketi baþarýyla güncellendi";
        }
    }
}