using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles
{
    [Permission("role:view")]
    public sealed record RoleGetQuery(
    Guid Id) : IRequest<Result<RoleDto>>;

    internal sealed class RoleGetQueryHandler(
        IRoleRepository roleRepository, IUserRepository userRepository) : IRequestHandler<RoleGetQuery, Result<RoleDto>>
    {
        public async Task<Result<RoleDto>> Handle(RoleGetQuery request, CancellationToken cancellationToken)
        {
            var res = await roleRepository
                .Where(i => i.Id == request.Id)
                 .MapToGet(userRepository.GetAll())
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
            {
                return Result<RoleDto>.Failure("Rol bulunamadı");
            }

            return res;
        }
    }
}
