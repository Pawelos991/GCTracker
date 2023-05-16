using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCTracker_Backend;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GC_Tracker_Datalayer.Context;
using GC_Tracker_Logic.Interfaces;
using GC_Tracker_Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace GCTracker_IntegrationTests.Provider
{
    public class TestClientProvider : IDisposable
    {
        private TestServer server;

        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = server.CreateClient();
        }

        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(Assembly.Load("GCTracker_Backend")).AddControllersAsServices();

            services.AddDbContext<GC_Tracker_Context>(
                options => options.UseNpgsql("Server=scrapplerdb.postgres.database.azure.com;Database=gc_tracker;Port=5432;User Id=scrappleruser;Password=123#@!crawler;Ssl Mode=Require;Trust Server Certificate=true"));
            services.AddScoped<IGpuServices, GpuServcies>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
