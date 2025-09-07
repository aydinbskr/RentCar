using RentCarServer.Domain.LoginTokens;
using System.Security.Claims;

namespace RentCarServer.WebAPI.Middlewares
{
    public sealed class CheckTokenMiddleware(
    ILoginTokenRepository loginTokenRepository) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                var token = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                if (string.IsNullOrWhiteSpace(token))
                {
                    await next(httpContext);
                    return;
                }

                var userValue = httpContext.User.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                    .Value;
                if (userValue is null)
                {
                    throw new TokenException();
                }

                Guid userId = Guid.Parse(userValue);

                var isTokenAvailable = await loginTokenRepository.AnyAsync(p =>
                    p.UserId == userId
                    && p.Token == token
                    && p.IsActive == true);
                if (!isTokenAvailable)
                {
                    throw new TokenException();
                }

                await next(httpContext);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }

    public sealed class TokenException : Exception;
}
