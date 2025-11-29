using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Configurations
{
    internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");
            builder.HasKey(x => x.Id);

            builder.OwnsMany(p => p.ReservationExtras);
            builder.OwnsMany(p => p.Histories);
            builder.OwnsOne(p => p.PaymentInformation);
            builder.OwnsOne(p => p.Status);
        }
    }
}
