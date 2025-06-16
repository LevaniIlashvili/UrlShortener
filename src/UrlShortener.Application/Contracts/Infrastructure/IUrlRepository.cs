using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Contracts.Infrastructure
{
    public interface IUrlRepository
    {
        Task<Url?> GetByShortCodeAsync(string shortCode);
        Task AddAsync(Url url);
        Task UpdateAsync(Url url);
        Task DeleteAsync(string shortCode);
    }
}
