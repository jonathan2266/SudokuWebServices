using Sudoku.Parser;
using Sudoku.Parser.Readers;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku.Scraper.Core.Repositories;
using Sudoku.Scraper.Core.Services.Version;

namespace Sudoku.Scraper.Core.Services.Strategies
{
    public class Web1SudokuStrategy : IStrategy
    {
        private readonly IProvideBoardNumber _provideBoardNumber;
        private readonly IRetrievePuzzle _retrievePuzzle;
        private readonly IReader _reader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotify<NewPuzzleFound> _newPuzzleNotification;

        private readonly int _expectedPuzzles = 1;

        public Web1SudokuStrategy(IProvideBoardNumber provideBoardNumber, IRetrievePuzzle retrievePuzzle, IReader reader, IUnitOfWork unitOfWork, INotify<NewPuzzleFound> notify)
        {
            _provideBoardNumber = provideBoardNumber;
            _retrievePuzzle = retrievePuzzle;
            _reader = reader;
            _unitOfWork = unitOfWork;
            _newPuzzleNotification = notify;
        }

        public async Task Execute()
        {
            var boardnumber = await _provideBoardNumber.Read(_reader);

            if (await _unitOfWork.RetrievedPuzzlesRepository.DoesBoardNumberExists(boardnumber))
            {
                return;
            }

            var boardPuzzle = await _retrievePuzzle.Load(_reader);
            if (boardPuzzle.Count() != _expectedPuzzles)
            {
                throw new InvalidDataException($"Recieved {boardPuzzle.Count()} puzzles instead of {_expectedPuzzles}");
            }

            var puzzle = boardPuzzle.First();

            await _newPuzzleNotification.Send(new NewPuzzleFound() { Boundary = new Parser.Utilities.UnorderedCellUtilities.Boundary(puzzle.Size), Puzzle = "TODO" });
            await _unitOfWork.RetrievedPuzzlesRepository.Add(boardnumber); //only after sucessfull publishing do we save the record.
            await _unitOfWork.CompleteAsync();
        }
    }
}
