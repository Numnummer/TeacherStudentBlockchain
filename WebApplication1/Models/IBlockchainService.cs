using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models;

public interface IBlockchainService
{
    void AddBlock(UserDisplayData user);
    public BlockDisplayInfo[]? GetBlocks(string resource);
}