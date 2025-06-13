using LoggingService.Models;
using LoggingService.Repositories;

namespace LoggingService
{
    public class Worker(ILogger<Worker> logger, ILogRepository logRepository) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly ILogRepository _logRepository = logRepository;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LoggingService started...");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
