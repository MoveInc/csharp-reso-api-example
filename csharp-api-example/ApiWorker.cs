using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace csharp_api_example
{
    public class ApiWorker : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ApiWorker(ILogger<BackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Force full on startup
            var prevTime = DateTime.Now.AddHours(-DateTime.Now.Hour);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                // Hour changed and new day meaning a full should be ran.
                if (now.Hour != prevTime.Hour && prevTime.Hour == 0)
                {
                    _logger.LogWarning("New day");
                    var full = new FullWorker(_logger, _serviceScopeFactory);
                    full.Execute();
                    prevTime = now;
                }
                // Hour changed, only process an incremental feed.
                else if (now.Hour != prevTime.Hour)
                {
                    _logger.LogWarning("New hour");
                    var incremental = new IncrementalWorker(_logger, _serviceScopeFactory);
                    incremental.Execute();
                    prevTime = now;
                }
                // No need to run
                else
                {
                    _logger.LogWarning("Not new hour");
                    // Use less time for more precision
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
