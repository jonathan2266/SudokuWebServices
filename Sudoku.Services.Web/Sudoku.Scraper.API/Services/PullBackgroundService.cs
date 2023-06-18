using Microsoft.Extensions.Options;
using Sudoku.Scraper.API.Configuration;
using Sudoku.Scraper.Core.UseCase.Download;
using System.Diagnostics;

namespace Sudoku.Scraper.API.Services
{
    public class PullBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly TimeSpan _timeout = new(0, 0, 1);
        private readonly ILogger<PullBackgroundService> _logger;
        private readonly PullOptions _pullOptions;

        private readonly List<Task> _downloadTasks = new();

        public PullBackgroundService(ILogger<PullBackgroundService> logger, IServiceProvider serviceprovider, IOptions<PullOptions> pullOptions)
        {
            _logger = logger;
            _serviceProvider = serviceprovider;
            _pullOptions = pullOptions.Value;
            _timeout = _pullOptions.RequestTimeSpan;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var time = Stopwatch.StartNew();

                    for (int i = 0; i < _pullOptions.Requests; i++)
                    {
                        var task = Task.Run(async () =>
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var orchistrator = scope.ServiceProvider.GetRequiredService<IDownloadOrchistrator>();
                            await orchistrator.Download();
                        }, stoppingToken);

                        _downloadTasks.Add(task);
                    }

                    await Task.WhenAll(_downloadTasks);
                    _downloadTasks.Clear();

                    await Task.Delay(CalculateRequiredTimeout(time.Elapsed), stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unexpected error while executing a pull.");
                }
            }
        }

        private TimeSpan CalculateRequiredTimeout(TimeSpan requestDuration)
        {
            if (requestDuration >= _timeout)
            {
                return TimeSpan.Zero;
            }


            return _timeout - requestDuration;
        }
    }
}
