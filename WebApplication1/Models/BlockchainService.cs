using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models;

public class BlockchainService:IBlockchainService
{
    public void AddBlock(UserDisplayData user)
    {
        var file = user.File;
        if (BlockSystem.Resources == null)
        {
            BlockSystem.Resources = [];
        }
        var resource = BlockSystem.Resources.FirstOrDefault(r => r.Subject == user.SelectedResource);
        if (resource != null)
        {
            resource.FilesBlockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey);
        }
        else
        {
            var blockchain=new FilesBlockchain();
            blockchain.CreateGenesisBlock();
            blockchain.AddBlock(user.Id, file.FileName, user.PublicKey, user.PrivateKey);
            BlockSystem.Resources.Add(new Resource()
            {
                Subject = user.SelectedResource,
                FilesBlockchain = blockchain
            });
        }
    }

    public BlockDisplayInfo[]? GetBlocks(string resource)
    {
        if (BlockSystem.Resources == null)
            return null;
        var res = BlockSystem.Resources.FirstOrDefault(r=>r.Subject == resource);
        return res?.FilesBlockchain.GetBlocks();
    }
}