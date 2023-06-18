using Sudoku.Scraper.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Scraper.DAL.Repositories
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly ScraperContext _scraperContext;

        private IRetrievedPuzzlesRepository? _retrievedPuzzlesRepository = null;


        public UnitOfwork(ScraperContext scraperContext)
        {
            _scraperContext = scraperContext;
        }

        public IRetrievedPuzzlesRepository RetrievedPuzzlesRepository
        {
            get
            {
                return _retrievedPuzzlesRepository ??= new WebSudokuRetrievedPuzzlesRepository(_scraperContext);
            }
        }

        public async Task CompleteAsync()
        {
            await _scraperContext.SaveChangesAsync();
        }
    }
}
