namespace Sudoku.Scraper.Core.Services.Strategies
{
    public interface IDownloadStrategyFactory
    {
        Task<IStrategy> Create();
    }
}
