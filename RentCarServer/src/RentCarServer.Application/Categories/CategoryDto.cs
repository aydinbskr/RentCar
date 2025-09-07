using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Categories
{
    public sealed class CategoryDto : EntityDto
    {
        public string Name { get; set; } = default!;
    }

    public static class CategoryExtensions
    {
        public static IQueryable<CategoryDto> MapToGet(this IQueryable<EntityWithAuditDto<Category>> entities)
        {
            return entities.Select(s => new CategoryDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name,
                IsActive = s.Entity.IsActive,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy,
                CreatedFullName = s.CreatedUser.FullName,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName : null,
            });
        }
    }
}
