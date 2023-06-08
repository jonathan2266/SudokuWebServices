using Autofac;
using Autofac.Extensions.DependencyInjection;
using Sudoku.Scraper.API.Configuration.Modules;

namespace Sudoku.Scraper.API.Extensions
{
    public static class ConfigureHostBuilderExtension
    {
        public static IHostBuilder ConfigureApplicationHost(this IHostBuilder builder)
        {
            builder.ConfigureAutofac();

            return builder;
        }

        public static IHostBuilder ConfigureAutofac(this IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ScraperModule()));

            return builder;
        }
    }
}
