﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Scraper.Core
{
    public interface IDownloadOrchistrator
    {
        public Task Download();
    }
}
