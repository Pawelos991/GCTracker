using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTracker_Scrawler.Config
{
    public static class ConfigReader
    {
        public static Config ReadAppSettings()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var connectionString = config["ConnectionString"];
            return new Config(connectionString);
        }
    }
}
