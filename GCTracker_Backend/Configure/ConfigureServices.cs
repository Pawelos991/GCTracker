using GC_Tracker_Datalayer.Context;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GCTracker_Backend.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection,
            ConfigurationManager configurationManager)
        {
            serviceCollection.AddDbContextFactory<GC_Tracker_Context>(
                options => options.UseNpgsql(configurationManager.GetValue<string>("ConnectionString")));
            serviceCollection.AddScoped<IGpuServices, GpuServcies>();

            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "GC_Tracker", Version = "v1" }));

            return serviceCollection;
        }
    }
}
