// See https://aka.ms/new-console-template for more information
using DbUp;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace GC_Tracker_DB_Versioning
{
    public class Program
    {
        static int Main(string[] args)
        {
            var myConnectionString = GetMyConnectionString();

            var connectionString = args.FirstOrDefault()  ?? myConnectionString;
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static string? GetMyConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();

            var myConnectionString = _configuration.GetSection("ConnectionString").Value;
            return myConnectionString;
        }
    }

}

