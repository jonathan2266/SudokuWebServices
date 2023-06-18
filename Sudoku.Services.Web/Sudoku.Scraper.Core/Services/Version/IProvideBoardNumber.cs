using Sudoku.Parser.Readers;
using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.Core.Services.Version
{
    public interface IProvideBoardNumber
    {
        Task<BoardNumber> Read(IReader reader);
    }
}
