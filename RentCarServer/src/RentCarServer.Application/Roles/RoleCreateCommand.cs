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
    [Permission("role:create")]
    public sealed record RoleCreateCommand(
    string Name,
    bool IsActive) : IRequest<Result<string>>;


    public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
    {
        public RoleCreateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir rol adı girin");
        }
    }

    internal sealed class RoleCreateCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RoleCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await roleRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (nameExists)
            {
                return Result<string>.Failure("Rol adı daha önce tanımlanmış");
            }

            Role role = new(request.Name, request.IsActive);
            roleRepository.Add(role);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Role başarıyla kaydedildi";
        }
    }
}
