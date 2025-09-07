using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches
{
    [Permission("branch:edit")]
    public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    Address Address,
    Contact Contact,
    bool IsActive
    ) : IRequest<Result<string>>;

    public sealed class BranchUpdateCommandValidator : AbstractValidator<BranchUpdateCommand>
    {
        public BranchUpdateCommandValidator()
        {
            RuleFor(i => i.Name).NotEmpty().WithMessage("Geçerli bir şube adı girin");
            RuleFor(i => i.Address.City).NotEmpty().WithMessage("Geçerli bir şehir seçin");
            RuleFor(i => i.Address.District).NotEmpty().WithMessage("Geçerli bir ilçe seçin");
            RuleFor(i => i.Address.FullAddress).NotEmpty().WithMessage("Geçerli bir tam adres girin");
            RuleFor(i => i.Contact.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
        }
    }

    internal sealed class BranchUpdateCommandHandler(
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<BranchUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
        {
            var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (branch is null)
            {
                return Result<string>.Failure("Şube bulunamadı");
            }


            branch.SetName(request.Name);
            branch.SetAddress(request.Address);
            branch.SetContact(request.Contact);
            branch.SetStatus(request.IsActive);

            branchRepository.Update(branch);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Şube bilgisi başarıyla güncellendi";
        }
    }
}
