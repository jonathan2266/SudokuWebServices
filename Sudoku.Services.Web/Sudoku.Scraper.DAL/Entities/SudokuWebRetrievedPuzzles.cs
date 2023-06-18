using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.DAL.Entities
{
    public class SudokuWebRetrievedPuzzles
    {
        public SudokuWebRetrievedPuzzles()
        {

        }

        public SudokuWebRetrievedPuzzles(BoardNumber boardNumber)
        {
            Id = boardNumber;
        }

        public BoardNumber Id { get; set; }

        public static SudokuWebRetrievedPuzzles FromKey(BoardNumber boardNumber)
        {
            return new SudokuWebRetrievedPuzzles(boardNumber);
        }
    }
}
