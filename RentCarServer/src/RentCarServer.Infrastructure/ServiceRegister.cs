using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Context;
using RentCarServer.Infrastructure.Options;
using RentCarServer.Infrastructure.Repositories;
using RentCarServer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["MailSettings:UserId"]))
            {
                int.TryParse(configuration["MailSettings:Port"], out int port);
                services.AddFluentEmail(configuration["MailSettings:Email"])
                    .AddSmtpSender(
                        configuration["MailSettings:Smtp"],
                        port);
            }
            else
            {
                int.TryParse(configuration["MailSettings:Port"], out int port);
                services.AddFluentEmail(configuration["MailSettings:ApiKey"])
                    .AddSmtpSender(
                        configuration["MailSettings:Smtp"],
                        port,
                        configuration["MailSettings:UserId"],
                        configuration["MailSettings:Password"]);
            }

            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                string con = configuration.GetConnectionString("SqlServer")!;
                opt.UseSqlServer(con);
            });

            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoginTokenRepository, LoginTokenRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProtectionPackageRepository, ProtectionPackageRepository>();

            services.AddScoped<IClaimContext, ClaimContext>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IMailService, MailService>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.ConfigureOptions<JwtSetupOptions>();

            services.AddAuthentication().AddJwtBearer();
            services.AddAuthorization();

            return services;
        }
    }
}
