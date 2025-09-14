using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Extras
{
    [Permission("extra:create")]
    public sealed record ExtraCreateCommand(
    string Name,
    decimal Price,
    string Description,
    bool IsActive) : IRequest<Result<string>>;

    public sealed class ExtraCreateCommandValidator : AbstractValidator<ExtraCreateCommand>
    {
        public ExtraCreateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir ekstra adý girin");
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz");
            RuleFor(p => p.Description).MaximumLength(500);
        }
    }

    internal sealed class ExtraCreateCommandHandler(
        IExtraRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<ExtraCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ExtraCreateCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await repository.AnyAsync(p => p.Name == request.Name, cancellationToken);
            if (nameExists)
                return Result<string>.Failure("Ekstra adý daha önce tanýmlanmýþ");

            var extra = new Extra(request.Name, request.Price, request.Description, request.IsActive);
            repository.Add(extra);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Ekstra baþarýyla kaydedildi";
        }
    }
}
