using Sudoku.Scraper.Core.Services.Strategies;

namespace Sudoku.Scraper.Core.UseCase.Download
{
    public class LimiterDownloadOrchastrator : IDownloadOrchistrator
    {
        private readonly IDownloadStrategyFactory _downloadStrategyFactory;

        public LimiterDownloadOrchastrator(IDownloadStrategyFactory downloadStrategyFactory)
        {
            _downloadStrategyFactory = downloadStrategyFactory;
        }

        public async Task Download()
        {
            await CreateDownloadTask();
        }

        private async Task CreateDownloadTask()
        {
            var newDownloadTask = await _downloadStrategyFactory.Create();
            await newDownloadTask.Execute();
        }
    }
}
