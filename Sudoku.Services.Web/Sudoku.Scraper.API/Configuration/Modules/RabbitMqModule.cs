using Autofac;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Sudoku.Scraper.Core.Common.Interfaces;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku_Scraper.RabbitMQ;
using Sudoku_Scraper.RabbitMQ.Senders;

namespace Sudoku.Scraper.API.Configuration.Modules
{
    public class RabbitMqModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<NewPuzzleFoundSender>().As<INotify<NewPuzzleFound>>().InstancePerLifetimeScope();

            builder.Register<IAsyncConnectionFactory>((context) =>
            {
                var connectionOptions = context.Resolve<IOptions<MessageBrokerConnectionOptions>>().Value;

                return new ConnectionFactory()
                {
                    Port = connectionOptions.Port,
                    HostName = connectionOptions.HostName,
                    UserName = connectionOptions.UserName,
                    Password = connectionOptions.Password,
                };
            }).SingleInstance();

            builder.Register<IConnection>((context) =>
            {
                var pool = context.Resolve<IConnectionPool>();
                return pool.GetConnection();
            }).ExternallyOwned().InstancePerLifetimeScope();

            builder.Register<IModel>((context) =>
            {
                var connection = context.Resolve<IConnection>();
                return connection.CreateModel();
            }).InstancePerLifetimeScope();

            builder.RegisterType<ConnectionPool>().As<IConnectionPool>().SingleInstance();
            builder.RegisterType<QueueInitialization>().As<IInit>().InstancePerLifetimeScope();
        }
    }
}
