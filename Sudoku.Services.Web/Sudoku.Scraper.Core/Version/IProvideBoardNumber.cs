using Sudoku.Parser.Readers;

namespace Sudoku.Scraper.Core.Version
{
    public interface IProvideBoardNumber
    {
        Task<BoardNumber> Read(IReader reader);
    }
}
