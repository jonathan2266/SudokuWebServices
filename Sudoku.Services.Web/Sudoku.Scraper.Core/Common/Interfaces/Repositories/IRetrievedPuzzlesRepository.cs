using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.Core.Common.Interfaces.Repositories
{
    public interface IRetrievedPuzzlesRepository
    {
        Task<bool> DoesBoardNumberExists(BoardNumber number);

        ValueTask Add(BoardNumber number);
    }
}
