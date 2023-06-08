using Autofac;
using Sudoku.Scraper.Core;
using Sudoku.Scraper.Core.Strategies;
using Sudoku.Scraper.Core.Version;

namespace Sudoku.Scraper.API.Configuration.Modules
{
    public class ScraperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<LimiterDownloadOrchastrator>().As<IDownloadOrchistrator>().InstancePerLifetimeScope();
            builder.RegisterType<ConfigurableDownloadStratefyFactory>().As<IDownloadStrategyFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SudokuBoardVersionProvider>().As<IProvideBoardNumber>().SingleInstance();
        }
    }
}
