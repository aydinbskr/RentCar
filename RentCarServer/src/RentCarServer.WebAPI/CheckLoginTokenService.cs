
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using System.Threading;

namespace RentCarServer.WebAPI
{
    public class CheckLoginTokenService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = scopeFactory.CreateScope();
            var loginTokenRepository = scope.ServiceProvider.GetRequiredService<ILoginTokenRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var now = DateTimeOffset.Now;

            await loginTokenRepository
                .Where(p => p.IsActive == true &&  p.ExpiresDate < now)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive, false), stoppingToken);

            await unitOfWork.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromDays(1));
        }
    }
}
