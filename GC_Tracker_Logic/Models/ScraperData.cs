using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Logic.Models
{
    public class ScraperData
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string? PhotoLink { get; set; }

        public string PageName { get; set; }

        public DateTime? Created { get; set; }

        public string? CreatedBy { get; set; }
    }
}
