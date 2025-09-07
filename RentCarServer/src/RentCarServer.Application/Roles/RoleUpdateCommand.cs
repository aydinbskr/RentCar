using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles
{
    [Permission("role:edit")]
    public sealed record RoleUpdateCommand(
    Guid Id,
    string Name,
    bool IsActive) : IRequest<Result<string>>;

    public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
    {
        public RoleUpdateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir rol adı girin");
        }
    }

    internal sealed class RoleUpdateCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (role is null)
            {
                return Result<string>.Failure("Rol bulunamadı");
            }

            role.SetName(request.Name);
            role.SetStatus(request.IsActive);
            roleRepository.Update(role);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Rol başarıyla güncellendi";
        }
    }
}
