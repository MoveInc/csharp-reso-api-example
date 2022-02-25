using System;
using csharp_api_example.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace csharp_api_example
{
    public static class Program
    {
        // Note: Warning logs are informational to keep the spam from LogInformation to a minimum.
        // Program starts by running full pull, then incremental every hour.
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
                .MigrateDatabase<PropertyContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Connection string should be in configuration
                    var connection = @"Server=db;Database=master;User=sa;Password=Your_password123;";
                    services.AddDbContext<PropertyContext>(
                        options => options.UseSqlServer(connection));

                    services.AddHostedService<ApiWorker>();
                    services.AddScoped<PropertyContext>();
                });

        // Migrates the database on application startup.
        // Original migration created with:
        // dotnet-ef migrations add <<Migration Name>> --output-dir ./data -p ./csharp-api-pull.csproj -s ./csharp-api-pull.csproj
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<BackgroundService>>();
                logger.LogWarning("Migrating DB");
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                    logger.LogWarning("Migration complete.");
                }
                catch (Exception e)
                {
                    logger.LogError("An error occured while migrating the DB." + e.ToString());
                }
            }
            return webHost;
        }
    }
}
