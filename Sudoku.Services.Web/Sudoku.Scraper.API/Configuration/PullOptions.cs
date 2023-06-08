namespace Sudoku.Scraper.API.Configuration
{
    public class PullOptions
    {
        public const string Pull = "pull";
        public int TimeOutAfterRequestInSeconds { get; set; } = 100;
    }
}
