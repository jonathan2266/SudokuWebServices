using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Scraper.Core.Configuration
{
    public class ScrapeOptions
    {
        public string EndPoint { get; set; } = "https://1sudoku.com/sudoku/medium";
        public EndpointType EndpointType { get; set; } = EndpointType.Web1Sudoku;

    }

    public enum EndpointType
    {
        Web1Sudoku
    }
}
