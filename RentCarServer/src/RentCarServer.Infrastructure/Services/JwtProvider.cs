using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentCarServer.Infrastructure.Services
{
    internal sealed class JwtProvider(ILoginTokenRepository loginTokenRepository,
        IUnitOfWork unitOfWork,
        IRoleRepository roleRepository,
        IBranchRepository branchRepository,
        IOptions<JwtOptions> options) : IJwtProvider
    {
        public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken)
        {
            var role = await roleRepository.FirstOrDefaultAsync(i => i.Id == user.RoleId, cancellationToken);
            var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == user.BranchId, cancellationToken);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("fullName", user.FirstName + " " + user.LastName),
                new Claim("email",user.Email),
                new Claim("role", role?.Name ?? string.Empty),
                new Claim("permissions", role is null ? "" : JsonSerializer.Serialize(role.Permissions.Select(s => s.Value).ToArray())),
                new Claim("branch", branch?.Name ?? string.Empty),
                new Claim("branchId", branch?.Id.ToString() ?? string.Empty)
            };

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var expires = DateTime.Now.AddDays(1);
            JwtSecurityToken securityToken = new(
                issuer:options.Value.Issuer,
                audience:options.Value.Audience,
                claims:claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(securityToken);

            LoginToken loginToken = new(token, user.Id, expires);
            loginTokenRepository.Add(loginToken);

            var loginTokens = await loginTokenRepository
                .Where(p => p.UserId == user.Id && p.IsActive == true)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive, false),cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return token;
        }
    }
}
