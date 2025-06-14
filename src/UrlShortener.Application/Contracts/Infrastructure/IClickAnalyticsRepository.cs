using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Contracts.Infrastructure
{
    public interface IClickAnalyticsRepository
    {
        Task<IEnumerable<ClickAnalytics>> GetClickAnalyticsByShortCode(string shortCode);
        Task AddAsync(ClickAnalytics clickAnalytics);
    }
}
