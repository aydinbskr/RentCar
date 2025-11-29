using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Users;
using System.Linq;

namespace RentCarServer.Application.ProtectionPackages
{
    public sealed class ProtectionCoverageDto
    {
        public string Name { get; set; } = default!;
    }

    public sealed class ProtectionPackageDto : EntityDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int OrderNumber { get; set; }
        public bool IsRecommended { get; set; }
        public List<string> Coverages { get; set; } = new();
    }

    public static class ProtectionPackageExtensions
    {
        public static IQueryable<ProtectionPackageDto> MapToGet(this IQueryable<EntityWithAuditDto<ProtectionPackage>> entities)
        {
            return entities.Select(s => new ProtectionPackageDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name,
                Price = s.Entity.Price,
                OrderNumber = s.Entity.OrderNumber,
                IsRecommended = s.Entity.IsRecommended,
                Coverages = s.Entity.Coverages.Select(c => c.Name ).ToList(),
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