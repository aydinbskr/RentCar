using RentCarServer.Application.Behaviours;
using RentCarServer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Permissions
{
    [Permission("permission:view")]
    public sealed record PermissionGetAllQuery : IRequest<Result<List<string>>>;

    internal sealed class PermissionGetAllQueryHandler(
        PermissionService permissionService) : IRequestHandler<PermissionGetAllQuery, Result<List<string>>>
    {
        public Task<Result<List<string>>> Handle(PermissionGetAllQuery request, CancellationToken cancellationToken)
            => Task.FromResult(Result<List<string>>.Succeed(permissionService.GetAll()));
    }
}
