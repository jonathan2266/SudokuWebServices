using Sudoku_Scraper.RabbitMQ;

namespace Sudoku.Scraper.API.Services
{
    public class InitializeWorkerQueues : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public InitializeWorkerQueues(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var initHelper = scope.ServiceProvider.GetRequiredService<IInit>();

                initHelper.Init();
            }

            return Task.CompletedTask;
        }
    }
}
