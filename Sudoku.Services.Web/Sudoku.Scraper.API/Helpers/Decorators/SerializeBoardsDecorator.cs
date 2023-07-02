
using Sudoku.Scraper.API.Configuration;
using Sudoku.Serialization;
using System.Diagnostics;

namespace Sudoku.Scraper.API.Helpers.Decorators
{
    public class SerializeBoardsDecorator : ISerializeBoards<SudokuBoard>
    {
        private readonly ISerializeBoards<SudokuBoard> _serializer;

        private readonly static ActivitySource _activitySource = new(ActivityKeys.Api);
        private const string _puzzleSizeKey = "Size";

        public SerializeBoardsDecorator(ISerializeBoards<SudokuBoard> serializer)
        {
            _serializer = serializer;
        }

        public string Serialize(SudokuBoard board)
        {
            using (var source = _activitySource.StartActivity("Puzzle.Serialize"))
            {
                source?.SetTag(_puzzleSizeKey, board.Size);
                return _serializer.Serialize(board);
            }
        }
    }
}
