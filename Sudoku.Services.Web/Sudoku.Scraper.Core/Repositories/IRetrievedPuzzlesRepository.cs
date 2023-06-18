using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.Core.Repositories
{
    public interface IRetrievedPuzzlesRepository
    {
        Task<bool> DoesBoardNumberExists(BoardNumber number);

        ValueTask Add(BoardNumber number);
    }
}
