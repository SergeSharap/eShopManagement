using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LoggingService.Models
{
    public class LogEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Level { get; set; } = "Info"; // Info, Warning, Error

        public string Message { get; set; } = string.Empty;
    }
}
