using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortener.Application.Contracts.Infrastructure;

namespace UrlShortener.Infrastructure.Services
{
    public class UrlExpirationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UrlExpirationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var urlRepository = scope.ServiceProvider.GetRequiredService<IUrlRepository>();

                    await urlRepository.DeactivateExpiredUrlsAsync();

                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
