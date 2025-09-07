﻿using RentCarServer.Application.Behaviours;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;

namespace RentCarServer.Application.Users
{
    [Permission("user:view")]
    public sealed record UserGetAllQuery : IRequest<IQueryable<UserDto>>;

    internal sealed class UserGetAllQueryHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IClaimContext claimContext,
        IBranchRepository branchRepository) : IRequestHandler<UserGetAllQuery, IQueryable<UserDto>>
    {
        public Task<IQueryable<UserDto>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        {
            var res = userRepository
               .GetAllWithAudit()
               .MapTo(roleRepository
               .GetAll(), branchRepository.GetAll());

            if (claimContext.GetRoleName() != "sys_admin")
            {
                res = res.Where(i => i.BranchId == claimContext.GetBranchId());
            }

            return Task.FromResult(res);
        }
           
    }
}
