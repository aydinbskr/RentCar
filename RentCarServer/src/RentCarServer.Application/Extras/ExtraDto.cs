using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Application.Extras
{
    public sealed class ExtraDto : EntityDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
    }

    public static class ExtraExtensions
    {
        public static IQueryable<ExtraDto> MapTo(this IQueryable<EntityWithAuditDto<Extra>> entities)
        {
            return entities.Select(s => new ExtraDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name,
                Price = s.Entity.Price,
                Description = s.Entity.Description,
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
