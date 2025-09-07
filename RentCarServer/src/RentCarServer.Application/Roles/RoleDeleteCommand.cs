using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles
{
    [Permission("role:delete")]
    public sealed record RoleDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

    internal sealed class RoleDeleteCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RoleDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (role is null)
            {
                return Result<string>.Failure("Rol bulunamadı");
            }

            role.Delete();
            roleRepository.Update(role);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Rol başarıyla silindi";
        }
    }
}
