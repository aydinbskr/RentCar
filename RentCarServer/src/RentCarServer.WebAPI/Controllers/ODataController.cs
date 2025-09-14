﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentCarServer.Application.Branches;
using RentCarServer.Application.Categories;
using RentCarServer.Application.Extras;
using RentCarServer.Application.ProtectionPackages;
using RentCarServer.Application.Roles;
using RentCarServer.Application.Users;
using TS.MediatR;

namespace RentCarServer.WebAPI.Controllers
{
    [Route("odata")]
    [ApiController]
    [EnableQuery]
    public class MainODataController : ODataController
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EnableLowerCamelCase();
            builder.EntitySet<BranchDto>("branches");
            builder.EntitySet<RoleDto>("roles");
            builder.EntitySet<UserDto>("users");
            builder.EntitySet<CategoryDto>("categories");
            builder.EntitySet<ProtectionPackageDto>("protection-packages");
            builder.EntitySet<ExtraDto>("extras");
            return builder.GetEdmModel();
        }

        [HttpGet("branches")]
        public IQueryable<BranchDto> Branches(ISender sender, CancellationToken cancellationToken = default)
            => sender.Send(new BranchGetAllQuery(), cancellationToken).Result;

        [HttpGet("roles")]
        public IQueryable<RoleDto> Roles(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new RoleGetAllQuery(), cancellationToken).Result;

        [HttpGet("users")]
        public IQueryable<UserDto> Users(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new UserGetAllQuery(), cancellationToken).Result;

        [HttpGet("categories")]
        public IQueryable<CategoryDto> Categories(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new CategoryGetAllQuery(), cancellationToken).Result;

        [HttpGet("protection-packages")]
        public IQueryable<ProtectionPackageDto> ProtectionPackages(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new ProtectionPackageGetAllQuery(), cancellationToken).Result;

        [HttpGet("extras")]
        public IQueryable<ExtraDto> Extras(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new ExtraGetAllQuery(), cancellationToken).Result;

    }
}
