using Sudoku.Parser;
using Sudoku.Parser.Readers;
using Sudoku.Scraper.Core.Common.Interfaces;
using Sudoku.Scraper.Core.Common.Interfaces.Repositories;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku.Scraper.Core.Services.Version;
using Sudoku.Serialization;
using System.Diagnostics;

namespace Sudoku.Scraper.Core.Services.Strategies
{
    public class Web1SudokuStrategy : IStrategy
    {
        private readonly IProvideBoardNumber _provideBoardNumber;
        private readonly IRetrievePuzzle _retrievePuzzle;
        private readonly IReader _reader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotify<NewPuzzleFound> _newPuzzleNotification;
        private readonly ISerializeBoards<SudokuBoard> _serializer;

        private readonly int _expectedPuzzles = 1;

        private readonly static ActivitySource _activitySource = new(ActivityKeys.Core);
        private const string _boardIdTag = "BoardId";
        private const string _puzzleExistsTag = "PuzzleUnique";

        public Web1SudokuStrategy(IProvideBoardNumber provideBoardNumber, IRetrievePuzzle retrievePuzzle, IReader reader, IUnitOfWork unitOfWork,
            INotify<NewPuzzleFound> notify, ISerializeBoards<SudokuBoard> serializeBoards)
        {
            _provideBoardNumber = provideBoardNumber;
            _retrievePuzzle = retrievePuzzle;
            _reader = reader;
            _unitOfWork = unitOfWork;
            _newPuzzleNotification = notify;
            _serializer = serializeBoards;
        }

        public async Task Execute()
        {
            using (var source = _activitySource.StartActivity("Puzzle.Process"))
            {
                var boardnumber = await _provideBoardNumber.Read(_reader);

                source?.SetTag(_boardIdTag, boardnumber.Id);

                if (await _unitOfWork.RetrievedPuzzlesRepository.DoesBoardNumberExists(boardnumber))
                {
                    source?.SetTag(_puzzleExistsTag, false);
                    return;
                }
                source?.SetTag(_puzzleExistsTag, true);


                var boardPuzzle = await LoadPuzzle();
                if (boardPuzzle.Count() != _expectedPuzzles)
                {
                    throw new InvalidDataException($"Recieved {boardPuzzle.Count()} puzzles instead of {_expectedPuzzles}");
                }

                var puzzle = boardPuzzle.First();

                await _newPuzzleNotification.Send(new NewPuzzleFound() { Puzzle = _serializer.Serialize(puzzle) });
                await _unitOfWork.RetrievedPuzzlesRepository.Add(boardnumber); //only after sucessfull publishing do we save the record.
                await _unitOfWork.CompleteAsync();
            }
        }

        private async Task<IEnumerable<SudokuBoard>> LoadPuzzle()
        {
            using (var loadSource = _activitySource.StartActivity("Puzzle.Load"))
            {
                return await _retrievePuzzle.Load(_reader);
            }
        }
    }
}
