using MongoDB.Bson;
using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models.Services;

public class BlockchainService(BlockSystemService blockSystemService):IBlockchainService
{
    public async Task AddBlock(UserDisplayData user)
    {
        var file = user.File;
        var resource = await blockSystemService.GetResourceAsync(user.SelectedResource);
        if (resource is { FilesBlockchain.Blocks.Count: > 0 })
        {
            resource.FilesBlockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey);
        }
        else
        {
            var blockchain=new FilesBlockchain();
            blockchain.CreateGenesisBlock();
            blockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey);
            resource=new Resource()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Subject = user.SelectedResource,
                FilesBlockchain = blockchain
            };
        }
        await blockSystemService.AddOrUpdateResourceAsync(resource);
    }

    public async Task<BlockDisplayInfo[]?> GetBlocks(string subject)
    {
        var resource = await blockSystemService.GetResourceAsync(subject);
        return resource?.FilesBlockchain.GetBlocks();
    }
}