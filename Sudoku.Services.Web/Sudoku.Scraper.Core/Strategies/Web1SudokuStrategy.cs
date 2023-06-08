using Sudoku.Parser;
using Sudoku.Parser.Readers;
using Sudoku.Scraper.Core.Version;

namespace Sudoku.Scraper.Core.Strategies
{
    public class Web1SudokuStrategy : IStrategy
    {
        private readonly IProvideBoardNumber _provideBoardNumber;
        private readonly IRetrievePuzzle _retrievePuzzle;
        private readonly IReader _reader;

        public Web1SudokuStrategy(IProvideBoardNumber provideBoardNumber, IRetrievePuzzle retrievePuzzle, IReader reader)
        {
            _provideBoardNumber = provideBoardNumber;
            _retrievePuzzle = retrievePuzzle;
            _reader = reader;
        }

        public async Task Execute()
        {
            var boardnumber = await _provideBoardNumber.Read(_reader);
            //Check if number exists in database.

            var boardPuzzle = await _retrievePuzzle.Load(_reader);

            //publish new puzzle and boardNumber

        }
    }
}
