using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

        public static IServiceCollection AddApplicationTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://192.168.1.16:3012");
                    });
                    builder.AddAspNetCoreInstrumentation();
                    builder.AddHttpClientInstrumentation();
                    builder.AddEntityFrameworkCoreInstrumentation();
                    builder.ConfigureResource(res =>
                    {
                        res.AddService("WebScraper");
                    });
                    builder.AddSource(Core.Configuration.ActivityKeys.ToArray().Concat(Configuration.ActivityKeys.ToArray()).ToArray());
                });

            return services;
        }

        public static IServiceCollection AddCustomHttpClientFactories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddHttpClient<FastHttpClient>()
                .ConfigurePrimaryHttpMessageHandler(x => {
                    var handler = new HttpClientHandler();
                    handler.MaxConnectionsPerServer = 200;
                    return handler;
                });

            return services;
        }
    }
}