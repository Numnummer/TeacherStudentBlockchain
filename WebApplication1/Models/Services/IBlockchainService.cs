using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models.Services;

public interface IBlockchainService
{
    Task AddBlock(UserDisplayData user, byte[] fileBytes);
    Task<BlockDisplayInfo[]?> GetBlocks(string subject);
}