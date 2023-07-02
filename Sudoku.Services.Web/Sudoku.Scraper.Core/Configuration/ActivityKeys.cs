using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Scraper.Core.Configuration
{
    public class ActivityKeys
    {
        public const string Core = "Sudoku.Scraper.Core";

        public static string[] ToArray()
        {
            return new string[]
            {
                Core
            };
        }
    }
}
