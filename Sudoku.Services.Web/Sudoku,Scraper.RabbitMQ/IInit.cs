using RabbitMQ.Client;

namespace Sudoku_Scraper.RabbitMQ
{
    public interface IInit
    {
        void Init();
    }

    public class QueueInitialization : IInit
    {
        private readonly IModel _channel;

        public QueueInitialization(IModel channel)
        {
            _channel = channel;
        }

        //TODO configure from configuration
        public void Init()
        {
            _channel.ExchangeDeclare(Exchanges.NewPuzzle, "fanout", true, false, null);
            _channel.QueueDeclare(Queues.PuzzleSolver, true, false, false, null);
            _channel.QueueBind(Queues.PuzzleSolver, Exchanges.NewPuzzle, string.Empty, null);
        }
    }
}