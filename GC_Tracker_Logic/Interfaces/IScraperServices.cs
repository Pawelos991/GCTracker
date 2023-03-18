using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Logic.Models;

namespace GC_Tracker_Logic.Interfaces
{
    public interface IScraperServices
    {
        Task<List<ScraperData>> GetScraperDataAsync();
    }
}
