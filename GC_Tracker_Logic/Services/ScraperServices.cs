using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace GC_Tracker_Logic.Services
{
    public class ScraperServices : IScraperServices
    {
        private readonly GC_Tracker_Context _context;

        public ScraperServices(GC_Tracker_Context context)
        {
            _context = context;
        }
        public async Task<List<ScraperData>> GetScraperDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
