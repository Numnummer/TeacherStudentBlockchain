using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplication1.Models.Blockchain;
using WebApplication1.Models.Options;

namespace WebApplication1.Models.Services;

public class BlockSystemService
{
    private readonly IMongoCollection<Resource> _resourcesCollection;
    
    public BlockSystemService(IOptionsMonitor<ResourcesOptions> resourcesOptions)
    {
        var client = new MongoClient(resourcesOptions.CurrentValue.ConnectionString);
        var database = client.GetDatabase(resourcesOptions.CurrentValue.DatabaseName);
        _resourcesCollection = database.GetCollection<Resource>(resourcesOptions.CurrentValue.CollectionName);
    }
    
    public async Task<Resource?> GetResourceAsync(string subject)
    {
        return await _resourcesCollection
            .Find(r => r.Subject == subject)
            .FirstOrDefaultAsync();
    }
    
    public async Task AddOrUpdateResourceAsync(Resource resource)
    {
        resource.LastUpdated = DateTime.UtcNow;
        
        var filter = Builders<Resource>.Filter.Eq(r => r.Subject, resource.Subject);
        var options = new ReplaceOptions { IsUpsert = true };
        
        await _resourcesCollection.ReplaceOneAsync(filter, resource, options);
    }

    public async Task<Block?> GetBlockByIdAsync(string blockId)
    {
        var filter = Builders<Resource>.Filter.ElemMatch(
            r => r.FilesBlockchain.Blocks,
            b => b.Id == blockId
        );

        var resource = await _resourcesCollection.Find(filter).FirstOrDefaultAsync();
        return resource?.FilesBlockchain?.Blocks?.FirstOrDefault(b => b.Id == blockId);
    }
}