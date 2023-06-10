using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Logic.Models;

namespace GC_Tracker_Logic.ML
{
    public class PredictPriceChange
    {
        public Task<PredicedPrice> GpuPriceChange(IList<GpuDto> HistoricalData)
        {
            var modelBuilder = new ModelBuilder();
            var model = modelBuilder.BuildModel(HistoricalData.Select(x => new ModelInput
            {
                PriceDiffrence = (float)x.Price,
                Id = x.Id
            }).ToList());
            var result = model.Predict();
            var predictedPrice = result.ForecastedPriceDiffrence[0];
            PricePrediction priceStatus;
            if ((float)HistoricalData.Last().Price < predictedPrice)
            {
                priceStatus = PricePrediction.Rise;
            }
            else if ((float)HistoricalData.Last().Price > predictedPrice)
            {
                priceStatus = PricePrediction.Fall;
            }
            else
            {
                priceStatus = PricePrediction.Stable;
            }

            return Task.FromResult(new PredicedPrice(predictedPrice, priceStatus));
        }
    }
}
