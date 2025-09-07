using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;

namespace RentCarServer.Application.Roles
{
    [Permission("role:view")]
    public sealed record RoleGetAllQuery : IRequest<IQueryable<RoleDto>>;

    internal sealed class RoleGetAllQueryHandler(
        IRoleRepository roleRepository, IUserRepository userRepository) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleDto>>
    {
        public Task<IQueryable<RoleDto>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken) =>
            Task.FromResult(roleRepository.GetAll().MapTo(userRepository.GetAll()).AsQueryable());
    }
}
