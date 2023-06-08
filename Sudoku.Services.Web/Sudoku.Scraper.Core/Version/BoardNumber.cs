namespace Sudoku.Scraper.Core.Version
{
    public readonly struct BoardNumber
    {
        public BoardNumber(string id)
        {
            Id = id;
        }

        public string Id { get; init; }
    }
}
