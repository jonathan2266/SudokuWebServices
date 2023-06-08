using Microsoft.Extensions.Options;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Core.Strategies;

namespace Sudoku.Scraper.Core
{
    public class LimiterDownloadOrchastrator : IDownloadOrchistrator
    {
        private readonly IDownloadStrategyFactory _downloadStrategyFactory;

        private readonly LimiterOptions _limiterOptions;

        private readonly List<Task> _downloadTasks = new();

        public LimiterDownloadOrchastrator(IOptions<LimiterOptions> limiterOptions, IDownloadStrategyFactory downloadStrategyFactory)
        {
            _limiterOptions = limiterOptions.Value;
            _downloadStrategyFactory = downloadStrategyFactory;
        }

        public async Task Download()
        {
            for (int i = 0; i < _limiterOptions.Requests; i++)
            {
                await CreateDownloadTask();
            }

            await Task.WhenAll(_downloadTasks);
            _downloadTasks.Clear();
        }

        private async Task CreateDownloadTask()
        {
            var newDownloadTask = await _downloadStrategyFactory.Create();
            _downloadTasks.Add(newDownloadTask.Execute());
        }
    }
}
