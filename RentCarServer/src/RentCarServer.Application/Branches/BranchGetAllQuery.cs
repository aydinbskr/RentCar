using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;

namespace RentCarServer.Application.Branches
{
    [Permission("branch:view")]
    public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

    internal sealed class BranchGetAllQueryHandler(
        IBranchRepository branchRepository,
        IUserRepository userRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
    {
        public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = branchRepository
                .GetAll()
                .MapTo(userRepository.GetAll());

            return Task.FromResult(response);
        }
    }
}
