using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Scraper.Core.Configuration
{
    public class LimiterOptions
    {
        public const string Limiter = "limiter";

        public int Requests { get; set; } = 1;
    }
}
