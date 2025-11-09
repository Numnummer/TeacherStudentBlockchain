using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models;

public class Resource
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string? Subject { get; set; }
    public string[]? Groups { get; set; }
    public FilesBlockchain FilesBlockchain { get; set; }
    [BsonDateTimeOptions]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}