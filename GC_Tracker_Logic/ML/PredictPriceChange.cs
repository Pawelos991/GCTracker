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
            return Task.FromResult(new PredicedPrice(predictedPrice, (float)HistoricalData.Last().Price < predictedPrice));
        }
    }
}
