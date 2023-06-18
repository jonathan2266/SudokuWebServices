using Microsoft.EntityFrameworkCore;
using Sudoku.Scraper.DAL.Entities;
using Sudoku.Scraper.Domain.Entities;

namespace Sudoku.Scraper.DAL
{
    public class ScraperContext : DbContext
    {
        public ScraperContext(DbContextOptions<ScraperContext> options) : base(options)
        {

        }

        public DbSet<SudokuWebRetrievedPuzzles> SudokuWebRetrievedPuzzles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SudokuWebRetrievedPuzzles>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .ValueGeneratedNever()
                .IsRequired(true)
                .HasConversion(v => v.Id, v => new BoardNumber(v));
            });
        }
    }
}
