using RabbitMQ.Client;

namespace Sudoku_Scraper.RabbitMQ.Senders
{
    public class SenderBase
    {
        private readonly IModel _model;

        protected readonly string _exchange = string.Empty;
        protected readonly string _routingKey = string.Empty;
        protected readonly IBasicProperties _basicProperties;

        public SenderBase(IModel model, string routingKey, string exchange)
        {
            _model = model;
            _routingKey = routingKey;
            _exchange = exchange;

            _basicProperties = _model.CreateBasicProperties();
            _basicProperties.Persistent = true;
        }

        protected ValueTask Send(Memory<byte> data)
        {
            _model.BasicPublish(_exchange, _routingKey, _basicProperties, data);
            return ValueTask.CompletedTask;
        }
    }
}
