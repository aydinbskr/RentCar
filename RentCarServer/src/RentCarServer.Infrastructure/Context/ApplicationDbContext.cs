﻿using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Users;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Context
{
    internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginToken> LoginTokens { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyGlobalFilters();
            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("varchar(MAX)");
            base.ConfigureConventions(configurationBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Entity>();

            HttpContextAccessor httpContextAccessor = new();
            string? userIdString =
                httpContextAccessor
                .HttpContext?
                .User
                .Claims
                .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?
                .Value;

            if (userIdString is null)
            {
                return base.SaveChangesAsync(cancellationToken);
            }

            Guid userId = Guid.Parse(userIdString);
            Guid identityId = userId;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.CreatedBy)
                        .CurrentValue = identityId;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                    {
                        entry.Property(p => p.DeletedAt)
                        .CurrentValue = DateTimeOffset.Now;
                        entry.Property(p => p.DeletedBy)
                        .CurrentValue = identityId;
                    }
                    else
                    {
                        entry.Property(p => p.UpdatedAt)
                            .CurrentValue = DateTimeOffset.Now;
                        entry.Property(p => p.UpdatedBy)
                        .CurrentValue = identityId;
                    }
                }

                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
