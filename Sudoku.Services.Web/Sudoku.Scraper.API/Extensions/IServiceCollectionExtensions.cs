using Sudoku.Scraper.API.Configuration;
using Sudoku.Scraper.API.Services;
using Sudoku.Scraper.Core.Configuration;
using System.Runtime.CompilerServices;

namespace Sudoku.Scraper.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureHostedServices(this IServiceCollection service)
        {
            return service.AddHostedService<PullBackgroundService>();
        }

        public static IServiceCollection ConfigureApplicationOptions(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions<PullOptions>();
            service.AddOptions<LimiterOptions>();
            service.AddOptions<ScrapeOptions>();


            return service;
        }
    }
}
