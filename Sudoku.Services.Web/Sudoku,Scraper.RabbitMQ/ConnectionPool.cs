using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Sudoku_Scraper.RabbitMQ
{
    public interface IConnectionPool
    {
        IConnection GetConnection();
    }

    public class ConnectionPool : IConnectionPool
    {
        private readonly IServiceProvider _serviceProvider;

        private IConnection _currentApplicationConnection = null;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ConnectionPool(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IConnection GetConnection()
        {
            if (_currentApplicationConnection == null || !_currentApplicationConnection.IsOpen)
            {
                CreateNewApplicationConnection();
            }

            ArgumentNullException.ThrowIfNull(_currentApplicationConnection);

            return _currentApplicationConnection;
        }

        private void CreateNewApplicationConnection()
        {
            try
            {
                _semaphore.Wait(); // Async await is not supported in a DI scenario + this should be a rare event.

                if (_currentApplicationConnection?.IsOpen == true)
                {
                    return;
                }

                var factory = _serviceProvider.GetRequiredService<IAsyncConnectionFactory>();
                _currentApplicationConnection = factory.CreateConnection();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
