using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Logic.Filters
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public decimal? PriceStart { get; set; }
        public decimal? PriceEnd { get; set; }
        public string ProducentCode { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }


    }
}
