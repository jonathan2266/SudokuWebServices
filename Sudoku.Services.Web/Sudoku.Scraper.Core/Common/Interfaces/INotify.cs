namespace Sudoku.Scraper.Core.Common.Interfaces
{
    public interface INotify<T> where T : class
    {
        ValueTask Send(T message);
    }
}
