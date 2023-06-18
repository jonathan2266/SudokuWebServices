using Sudoku.Scraper.Core.UseCase.Download;
using Sudoku.Scraper.DAL;

namespace Sudoku.Scraper.API.Services
{
    public class TransactionalOrchistrator : IDownloadOrchistrator
    {
        private readonly IDownloadOrchistrator _downloadOrchistrator;
        private readonly ScraperContext _context;

        public TransactionalOrchistrator(IDownloadOrchistrator downloadOrchistrator, ScraperContext scraperContext)
        {
            _downloadOrchistrator = downloadOrchistrator;
            _context = scraperContext;
        }

        public async Task Download()
        {
            var tr = await _context.Database.BeginTransactionAsync();

            await _downloadOrchistrator.Download();

            await tr.CommitAsync();
        }
    }
}
