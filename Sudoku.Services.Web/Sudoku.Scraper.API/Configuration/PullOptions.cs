namespace Sudoku.Scraper.API.Configuration
{
    public class PullOptions
    {
        public const string Pull = "pull";
        public int RequestTimeSpanInSeconds { get; set; } = 1;
        public int Requests { get; set; } = 10;

        public TimeSpan RequestTimeSpan
        {
            get
            {
                return new TimeSpan(0, 0, RequestTimeSpanInSeconds);
            }
        }
    }
}
