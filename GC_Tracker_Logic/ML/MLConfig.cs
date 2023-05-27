using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Logic.ML
{
    public class MLConfig
    {
        public int Window_Size { get; set; }
        public int Series_Length { get; set; }
        public int Horizon { get; set; }
        public float Confidence_Level { get; set; }
    }
}
