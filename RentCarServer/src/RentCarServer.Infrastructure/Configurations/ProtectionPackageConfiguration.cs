using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.ProtectionPackages;

namespace RentCarServer.Infrastructure.Configurations
{
    internal sealed class ProtectionPackageConfiguration : IEntityTypeConfiguration<ProtectionPackage>
    {
        public void Configure(EntityTypeBuilder<ProtectionPackage> builder)
        {
            builder.ToTable("ProtectionPackages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsRecommended)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.OwnsMany(x => x.Coverages);
        }
    }
}