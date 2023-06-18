using Microsoft.EntityFrameworkCore;
using Sudoku.Scraper.DAL;

namespace Sudoku.Scraper.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> MigrateDatabase(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ScraperContext>();
                await context.Database.MigrateAsync();

                return webApplication;
            }
        }
    }
}
