using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Cassandra
{
    public class CassandraDbContext
    {
        private readonly ICluster _cluster;
        private readonly ISession _session;
        private readonly ILogger<CassandraDbContext> _logger;
        private readonly CassandraSettings _settings;

        public CassandraDbContext(IOptions<CassandraSettings> settings, ILogger<CassandraDbContext> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            MappingConfiguration.Global.Define(
                new Map<Url>()
                    .TableName("urls")
                    .PartitionKey(u => u.ShortCode)
                    .Column(u => u.ShortCode, cm => cm.WithName("short_code"))
                    .Column(u => u.OriginalUrl, cm => cm.WithName("original_url"))
                    .Column(u => u.CreatedAt, cm => cm.WithName("created_at"))
                    .Column(u => u.ExpirationDate, cm => cm.WithName("expiration_date"))
                    .Column(u => u.ClickCount, cm => cm.WithName("click_count"))
                    .Column(u => u.IsActive, cm => cm.WithName("is_active")),

                new Map<ClickAnalytics>()
                    .TableName("click_analytics")
                    .PartitionKey(ca => ca.ShortCode)
                    .ClusteringKey(ca => ca.ClickDate)
                    .Column(ca => ca.ShortCode, cm => cm.WithName("short_code"))
                    .Column(ca => ca.ClickDate, cm => cm.WithName("click_date"))
                    .Column(ca => ca.UserAgent, cm => cm.WithName("user_agent"))
                    .Column(ca => ca.IpAddress, cm => cm.WithName("ip_address"))
            );

            _logger.LogInformation("Connecting to Cassandra at {ContactPoints} with Keyspace {KeyspaceName}",
                _settings.ContactPoints, _settings.KeyspaceName);

            try
            {
                _cluster = Cluster.Builder()
                    .AddContactPoints(_settings.ContactPoints.Split(','))
                    .WithPort(_settings.Port)
                    //.WithCredentials(_settings.Username, _settings.Password) // If authentication is enabled
                    .Build();

                _session = _cluster.Connect(_settings.KeyspaceName);
                _logger.LogInformation("Successfully connected to Cassandra and keyspace {KeyspaceName}", _settings.KeyspaceName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to Cassandra.");
                throw; // Re-throw the exception to indicate a critical startup failure
            }
        }

        public ISession GetSession() => _session;   

        public void Dispose()
        {
            _cluster?.Dispose();
            _session?.Dispose();
            _logger.LogInformation("Cassandra connection disposed.");
        }
    }

    public class CassandraSettings
    {
        public string ContactPoints { get; set; } = "localhost";
        public int Port { get; set; } = 9042;
        public string KeyspaceName { get; set; } = "url_shortener";
        //public string Username { get; set; } = ""; // Add if authentication is needed
        //public string Password { get; set; } = ""; // Add if authentication is needed
    }
}