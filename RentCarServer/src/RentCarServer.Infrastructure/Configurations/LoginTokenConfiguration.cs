using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.LoginTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Configurations
{
    internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
    {
        public void Configure(EntityTypeBuilder<LoginToken> builder)
        {
            builder.ToTable("LoginTokens");
            builder.HasKey(x => x.Id);
        }
    }
}
