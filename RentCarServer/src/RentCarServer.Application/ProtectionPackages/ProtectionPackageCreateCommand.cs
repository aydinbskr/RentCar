using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.ProtectionPackages;
using System.Linq;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.ProtectionPackages
{
    [Permission("protectionPackage:create")]
    public sealed record ProtectionPackageCreateCommand(
        string Name,
        decimal Price,
        bool IsRecommended,
        int OrderNumber,
        IEnumerable<string> Coverages,
        bool IsActive) : IRequest<Result<string>>;

    public sealed class ProtectionPackageCreateCommandValidator : AbstractValidator<ProtectionPackageCreateCommand>
    {
        public ProtectionPackageCreateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir paket adý girin");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Geçerli bir fiyat girin");
            RuleFor(p => p.Price).GreaterThan(-1).WithMessage("Fiyat pozitif olmalý");
        }
    }

    internal sealed class ProtectionPackageCreateCommandHandler(
        IProtectionPackageRepository protectionPackageRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProtectionPackageCreateCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await protectionPackageRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);
            if (nameExists)
            {
                return Result<string>.Failure("Koruma paketi adý daha önce tanýmlanmýþ");
            }

            var coverages = (request.Coverages ?? Enumerable.Empty<string>()).Select(c => new ProtectionCoverage(c));
            var package = new ProtectionPackage(request.Name, request.Price, request.IsRecommended, request.OrderNumber,coverages, request.IsActive);
            protectionPackageRepository.Add(package);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Koruma paketi baþarýyla kaydedildi";
        }
    }
}