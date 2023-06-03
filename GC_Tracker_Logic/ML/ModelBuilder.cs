using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries; 

namespace GC_Tracker_Logic.ML
{
    public class ModelBuilder
    {
        private MLConfig _config;

        public ModelBuilder()
        {
        }

        public TimeSeriesPredictionEngine<ModelInput, ModelOutput> BuildModel(IList<ModelInput> inputs)
        {
            //Create model
            var mlContext = new MLContext();
            var trainDataVeiw = mlContext.Data.LoadFromEnumerable(inputs);
            var windowsSize = inputs.Count()/2 < 2  ? 2 : inputs.Count()/2;
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedPriceDiffrence",
                inputColumnName: "PriceDiffrence",
                windowSize: windowsSize,
                seriesLength: 120,
                trainSize: inputs.Count,
                horizon: 30,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundPriceDiffrence",
                confidenceUpperBoundColumn: "UpperBoundPriceDiffrence");

            var forecaster = forecastingPipeline.Fit(trainDataVeiw);
            return forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
        }
    }

    public record ModelInput
    {
        public float PriceDiffrence { get; init; }
        public int Id { get; init; }
    }

    public record ModelOutput
    {
        public float[] ForecastedPriceDiffrence { get; init; }
        public float[] LowerBoundPriceDiffrence { get; init; }
        public float[] UpperBoundPriceDiffrence { get; init; }
    }
}
