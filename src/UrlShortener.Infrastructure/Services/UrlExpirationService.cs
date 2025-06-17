using Microsoft.Extensions.Hosting;
using UrlShortener.Application.Contracts.Infrastructure;

namespace UrlShortener.Infrastructure.Services
{
    public class UrlExpirationService : BackgroundService
    {
        private readonly IUrlRepository _urlRepository;

        public UrlExpirationService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _urlRepository.DeactivateExpiredUrlsAsync();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
