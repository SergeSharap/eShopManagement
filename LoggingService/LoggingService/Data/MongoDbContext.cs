using LoggingService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoggingService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<LogEntry> Logs => _database.GetCollection<LogEntry>("Logs");
    }
}
