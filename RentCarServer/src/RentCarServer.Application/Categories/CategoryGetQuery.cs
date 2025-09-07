﻿using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories
{
    [Permission("category:view")]
    public sealed record CategoryGetQuery(
    Guid Id) : IRequest<Result<CategoryDto>>;

    internal sealed class CategoryGetQueryHandler(
        ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetQuery, Result<CategoryDto>>
    {
        public async Task<Result<CategoryDto>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
        {
            var res = await categoryRepository
                .GetAllWithAudit()
                .MapToGet()
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (res is null)
            {
                return Result<CategoryDto>.Failure("Kategori bulunamadı");
            }

            return res;
        }
    }
}
