using LoggingService.Data;
using LoggingService.Models;
using MongoDB.Driver;

namespace LoggingService.Repositories
{
    public interface ILogRepository
    {
        Task AddLogAsync(LogEntry log);
        Task<IEnumerable<LogEntry>> GetLogsAsync();
    }

    public class LogRepository(MongoDbContext context) : ILogRepository
    {
        private readonly MongoDbContext _context = context;

        public async Task AddLogAsync(LogEntry log)
        {
            await _context.Logs.InsertOneAsync(log);
        }

        public async Task<IEnumerable<LogEntry>> GetLogsAsync()
        {
            return await _context.Logs.Find(_ => true).ToListAsync();
        }
    }
}
