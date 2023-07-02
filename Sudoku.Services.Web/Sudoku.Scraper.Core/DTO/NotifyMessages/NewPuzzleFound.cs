using static Sudoku.Parser.Utilities.UnorderedCellUtilities;

namespace Sudoku.Scraper.Core.DTO.NotifyMessages
{
    public class NewPuzzleFound
    {
        public string Puzzle { get; init; } = string.Empty;
    }
}