using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Contracts.Infrastructure;
using UrlShortener.Infrastructure.Cassandra;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CassandraSettings>(configuration.GetSection("Cassandra"));

            services.AddSingleton<CassandraDbContext>();

            services.AddSingleton<IBase62Encoder, Base62Encoder>();

            services.AddScoped<IUrlRepository, UrlRepository>();
            services.AddScoped<IClickAnalyticsRepository, ClickAnalyticsRepository>();

            services.AddHostedService<CassandraMigrationService>();
            return services;
        }
    }
}
