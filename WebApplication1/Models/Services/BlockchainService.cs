using System.IO.Pipelines;
using MongoDB.Bson;
using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models.Services;

public class BlockchainService(BlockSystemService blockSystemService):IBlockchainService
{
    private async Task<byte[]> CopyFileToByteArrayAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
    
        // Сбрасываем поток на начало если нужно
        if (file.Length > 0)
        {
            await file.CopyToAsync(memoryStream);
        
            // Проверяем что данные скопированы
            if (memoryStream.Length == 0)
            {
                throw new InvalidOperationException("Failed to copy file data");
            }
        
            return memoryStream.ToArray();
        }
    
        throw new InvalidOperationException("File is empty");
    }
    
    public async Task AddBlock(UserDisplayData user, byte[] fileBytes)
    {
        var file = user.File;
        
        var resource = await blockSystemService.GetResourceAsync(user.SelectedResource);
        if (resource is { FilesBlockchain.Blocks.Count: > 0 })
        {
            resource.FilesBlockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey, fileBytes);
        }
        else
        {
            var blockchain=new FilesBlockchain();
            blockchain.CreateGenesisBlock();
            blockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey, fileBytes);
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