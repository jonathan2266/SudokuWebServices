using Autofac;
using Sudoku.Scraper.API.Services;
using Sudoku.Scraper.Core.Repositories;
using Sudoku.Scraper.Core.Services.Strategies;
using Sudoku.Scraper.Core.Services.Version;
using Sudoku.Scraper.Core.UseCase.Download;
using Sudoku.Scraper.DAL.Repositories;

namespace Sudoku.Scraper.API.Configuration.Modules
{
    public class ScraperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConfigurableDownloadStrategyFactory>().As<IDownloadStrategyFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SudokuBoardVersionProvider>().As<IProvideBoardNumber>().SingleInstance();


            builder.RegisterType<LimiterDownloadOrchastrator>().As<IDownloadOrchistrator>().InstancePerLifetimeScope();
            builder.RegisterDecorator<TransactionalOrchistrator, IDownloadOrchistrator>();

            builder.RegisterType<UnitOfwork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
