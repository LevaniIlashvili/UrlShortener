using Cassandra;
using Cassandra.Mapping;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Cassandra;

namespace UrlShortener.Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public UrlRepository(CassandraDbContext dbContext)
        {
            _session = dbContext.GetSession();
            _mapper = new Mapper(_session);
        }

        public async Task<Url?> GetByShortCodeAsync(string shortCode)
        {
            var url = await _mapper.SingleOrDefaultAsync<Url>(
                @"SELECT short_code, original_url, created_at, expiration_date, click_count, is_active
                  FROM urls WHERE short_code = ?", shortCode
                );
            return url;
        }

        public async Task<Url?> GetByOriginalUrlAsync(string originalUrl)
        {
            var url = await _mapper.SingleOrDefaultAsync<Url>("SELECT * FROM urls WHERE original_url = ?", originalUrl);
            return url;
        }

        public async Task IncreaseClickCount(string shortCode)
        {
            await _mapper.ExecuteAsync("UPDATE urls SET click_count = click_count + 1 WHERE short_code = ?", shortCode);
        }

        public async Task AddAsync(Url url)
        {
            await _mapper.InsertAsync(url);
        }

        public async Task UpdateAsync(Url url)
        {

            await _mapper.UpdateAsync(url);
        }

        public async Task DeleteAsync(string shortCode)
        {

            await _mapper.DeleteAsync<Url>("WHERE short_code = ?", shortCode);
        }
    }
}