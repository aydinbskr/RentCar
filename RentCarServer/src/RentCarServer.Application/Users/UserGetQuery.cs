﻿using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Users
{
    [Permission("user:view")]
    public sealed record UserGetQuery(
    Guid Id) : IRequest<Result<UserDto>>;

    internal sealed class UserGetQueryHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IBranchRepository branchRepository) : IRequestHandler<UserGetQuery, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(UserGetQuery request, CancellationToken cancellationToken)
        {
            var res = await userRepository
                .GetAllWithAudit()
                .MapTo(roleRepository.GetAll(), branchRepository.GetAll())
                .Where(i => i.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
            {
                return Result<UserDto>.Failure("Kullanıcı bulunamadı");
            }

            return res;
        }
    }
}
