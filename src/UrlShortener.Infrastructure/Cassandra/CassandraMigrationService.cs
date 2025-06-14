using Cassandra;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UrlShortener.Infrastructure.Cassandra
{
    public class CassandraMigrationService : IHostedService
    {
        private readonly ILogger<CassandraMigrationService> _logger;
        private readonly CassandraSettings _settings;
        private ICluster? _cluster;
        private ISession? _session;

        public CassandraMigrationService(IOptions<CassandraSettings> settings, ILogger<CassandraMigrationService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Cassandra migration service...");

            ISession? initialSession = null;
            try
            {
                _cluster = Cluster.Builder()
                    .AddContactPoints(_settings.ContactPoints.Split(','))
                    .WithPort(_settings.Port)
                    //.WithCredentials(_settings.Username, _settings.Password)
                    .Build();

                initialSession = await _cluster.ConnectAsync();

                _logger.LogInformation("Ensuring keyspace '{KeyspaceName}' exists...", _settings.KeyspaceName);
                await initialSession.ExecuteAsync(
                    new SimpleStatement($"CREATE KEYSPACE IF NOT EXISTS {_settings.KeyspaceName} WITH REPLICATION = {{'class' : 'SimpleStrategy', 'replication_factor' : 1}}")
                );
                _logger.LogInformation("Keyspace '{KeyspaceName}' ensured.", _settings.KeyspaceName);

                initialSession.Dispose();
                _session = await _cluster.ConnectAsync(_settings.KeyspaceName);

                _logger.LogInformation("Connected to keyspace '{KeyspaceName}'. Applying table migrations...", _settings.KeyspaceName);

                await _session.ExecuteAsync(
                    new SimpleStatement(@"
                        CREATE TABLE IF NOT EXISTS urls (
                            short_code text PRIMARY KEY,
                            original_url text,
                            created_at timestamp,
                            expiration_date timestamp,
                            click_count bigint,
                            is_active boolean
                        )"
                    )
                );
                _logger.LogInformation("Table 'urls' ensured.");

                await _session.ExecuteAsync(
                    new SimpleStatement(@"
                        CREATE TABLE IF NOT EXISTS click_analytics (
                            short_code text,
                            click_date timestamp,
                            user_agent text,
                            ip_address text,
                            PRIMARY KEY ((short_code), click_date)
                        ) WITH CLUSTERING ORDER BY (click_date DESC)"
                    )
                );
                _logger.LogInformation("Table 'click_analytics' ensured.");

                _logger.LogInformation("Cassandra migration service completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cassandra migration failed. Application startup will be halted.");
                initialSession?.Dispose();
                _session?.Dispose();
                _cluster?.Dispose();
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Cassandra migration service.");
            _session?.Dispose();
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
