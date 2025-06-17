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
        private readonly Mapper _mapper;

        public UrlRepository(CassandraDbContext dbContext)
        {
            _session = dbContext.GetSession();
            _mapper = new Mapper(_session);
        }
        
        public async Task DeactivateExpiredUrlsAsync()
        {
            
            var selectQuery = "SELECT short_code FROM urls WHERE is_active = true AND expiration_date <= toTimestamp(now()) ALLOW FILTERING";
            var rows = await _session.ExecuteAsync(new SimpleStatement(selectQuery));

            foreach (var row in rows)
            {
                var shortCode = row.GetValue<string>("short_code");
                var updateQuery = "UPDATE urls SET is_active = false WHERE short_code = ?";
                await _session.ExecuteAsync(new SimpleStatement(updateQuery, shortCode));
            }
        }

        public async Task<Url?> GetByShortCodeAsync(string shortCode)
        {
            var url = await _mapper.SingleOrDefaultAsync<Url>(
                @"SELECT short_code, original_url, created_at, expiration_date, click_count, is_active
                  FROM urls WHERE short_code = ?", shortCode
                );
            return url;
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