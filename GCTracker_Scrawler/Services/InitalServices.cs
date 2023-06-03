using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GCTracker_Scrawler.Services
{
    public static class InitalServices
    {
        public static IServiceCollection InitDataContext(this ServiceCollection serviceCollection,
            string connectionString)
        {
            return serviceCollection.AddDbContextFactory<GC_Tracker_Context>(options =>
                options.UseNpgsql(connectionString));
        }

        public static IServiceCollection InitDataManipulationServices(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IImageServices, ImageServices>();
            return serviceCollection.AddScoped<IProductSevices, ProductServices>();
        }
    }
}
