namespace Sudoku.Scraper.Core.Services
{
    public interface INotify<T> where T : class
    {
        ValueTask Send(T message);
    }
}
