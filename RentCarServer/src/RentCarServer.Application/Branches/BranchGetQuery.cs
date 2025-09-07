using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches
{
    [Permission("branch:view")]
    public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<BranchDto>>;

    internal sealed class BranchGetQueryHandler(
        IBranchRepository branchRepository, IUserRepository userRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
    {
        public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
        {
            var branch = await branchRepository
                .Where(i => i.Id == request.Id)
                 .MapTo(userRepository.GetAll())
                .FirstOrDefaultAsync(cancellationToken);

            if (branch is null)
            {
                return Result<BranchDto>.Failure("Şube bulunamadı");
            }

            return branch;
        }
    }
}
