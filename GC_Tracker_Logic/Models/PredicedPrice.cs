using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Logic.Models
{
    public class PredicedPrice
    {
        public float PredictedPrice { get; set; }
        public bool IsPriceRising { get; set; }

        public PredicedPrice(float predictedPrice, bool isPriceRising)
        {
            PredictedPrice = predictedPrice;
            IsPriceRising = isPriceRising;
        }
    }
}
