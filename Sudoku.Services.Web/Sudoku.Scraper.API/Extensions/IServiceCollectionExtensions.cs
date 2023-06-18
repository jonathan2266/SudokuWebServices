using Microsoft.EntityFrameworkCore;
using Sudoku.Scraper.API.Configuration;
using Sudoku.Scraper.API.Services;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.DAL;

namespace Sudoku.Scraper.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureHostedServices(this IServiceCollection service)
        {
            service.AddHostedService<InitializeWorkerQueues>();
            return service.AddHostedService<PullBackgroundService>();
        }

        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions<PullOptions>();
            service.AddOptions<ScrapeOptions>();
            service.AddOptions<MessageBrokerConnectionOptions>();

            return service;
        }

        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ScraperContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.ScraperConnectionString)));

            return services;
        }
    }
}