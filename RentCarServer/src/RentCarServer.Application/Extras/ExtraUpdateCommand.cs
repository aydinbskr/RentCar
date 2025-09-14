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
    [Permission("extra:update")]
    public sealed record ExtraUpdateCommand(
    Guid Id,
    string Name,
    decimal Price,
    string Description,
    bool IsActive) : IRequest<Result<string>>;

    public sealed class ExtraUpdateCommandValidator : AbstractValidator<ExtraUpdateCommand>
    {
        public ExtraUpdateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir ekstra adý girin");
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz");
            RuleFor(p => p.Description).MaximumLength(500);
        }
    }

    internal sealed class ExtraUpdateCommandHandler(
        IExtraRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<ExtraUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ExtraUpdateCommand request, CancellationToken cancellationToken)
        {
            var extra = await repository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (extra is null)
                return Result<string>.Failure("Ekstra bulunamadý");

            if (!string.Equals(extra.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await repository.AnyAsync(
                    p => p.Name == request.Name && p.Id != request.Id,
                    cancellationToken);

                if (nameExists)
                    return Result<string>.Failure("Ekstra adý daha önce tanýmlanmýþ");
            }

            extra.SetName(request.Name);
            extra.SetPrice(request.Price);
            extra.SetDescription(request.Description);
            extra.SetStatus(request.IsActive);

            repository.Update(extra);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Ekstra baþarýyla güncellendi";
        }
    }
}
