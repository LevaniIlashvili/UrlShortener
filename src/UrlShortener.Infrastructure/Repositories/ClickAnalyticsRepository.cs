using Cassandra;
using Cassandra.Mapping;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Cassandra;

namespace UrlShortener.Infrastructure.Repositories
{
    public class ClickAnalyticsRepository : IClickAnalyticsRepository
    {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public ClickAnalyticsRepository(CassandraDbContext dbContext)
        {
            _session = dbContext.GetSession();
            _mapper = new Mapper(_session);
        }

        public async Task AddAsync(ClickAnalytics clickAnalytics)
        {
            await _mapper.InsertAsync(clickAnalytics);
        }

        public async Task<IEnumerable<ClickAnalytics>> GetClickAnalyticsByShortCode(string shortCode)
        {
            return await _mapper.FetchAsync<ClickAnalytics>("SELECT * FROM click_analytics WHERE short_code = ?", shortCode);
        }
    }
}
