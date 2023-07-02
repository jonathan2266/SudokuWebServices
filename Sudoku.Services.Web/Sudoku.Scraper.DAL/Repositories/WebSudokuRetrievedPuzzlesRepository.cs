using Microsoft.EntityFrameworkCore;
using Sudoku.Scraper.Core.Common.Interfaces.Repositories;
using Sudoku.Scraper.DAL.Entities;
using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.DAL.Repositories
{
    public class WebSudokuRetrievedPuzzlesRepository : IRetrievedPuzzlesRepository
    {
        private readonly ScraperContext _scraperContext;

        public WebSudokuRetrievedPuzzlesRepository(ScraperContext scraperContext)
        {
            _scraperContext = scraperContext;
        }

        public Task<bool> DoesBoardNumberExists(BoardNumber number)
        {
            return _scraperContext.SudokuWebRetrievedPuzzles.AnyAsync(x => x.Id == number);
        }

        public ValueTask Add(BoardNumber number)
        {
            _scraperContext.SudokuWebRetrievedPuzzles.Add(SudokuWebRetrievedPuzzles.FromKey(number));
            return ValueTask.CompletedTask;
        }
    }
}
