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
            _channel.ExchangeDeclare("new_puzzle", "fanout", true, false, null);
            _channel.QueueDeclare("puzzle_solver", true, false, false, null);
            _channel.QueueBind("puzzle_solver", "new_puzzle", string.Empty, null);
        }
    }
}