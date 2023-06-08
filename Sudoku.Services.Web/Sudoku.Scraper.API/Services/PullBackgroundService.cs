using Microsoft.Extensions.Options;
using Sudoku.Scraper.API.Configuration;
using Sudoku.Scraper.Core;

namespace Sudoku.Scraper.API.Services
{
    public class PullBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly TimeSpan _timeout = new(0, 0, 1);
        private readonly ILogger<PullBackgroundService> _logger;
        private readonly PullOptions _pullOptions;

        public PullBackgroundService(ILogger<PullBackgroundService> logger, IServiceProvider serviceprovider, IOptions<PullOptions> pullOptions)
        {
            _logger = logger;
            _serviceProvider = serviceprovider;
            _pullOptions = pullOptions.Value;
            _timeout = new TimeSpan(0, 0, _pullOptions.TimeOutAfterRequestInSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orchistrator = scope.ServiceProvider.GetRequiredService<IDownloadOrchistrator>();
                        await orchistrator.Download();
                    }

                    await Task.Delay(_timeout, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unexpected error while executing a pull.");
                }
            }
        }
    }
}
