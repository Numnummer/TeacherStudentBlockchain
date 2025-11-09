namespace WebApplication1.Models.Blockchain;

public class BlockDisplayInfo
{
    public Guid Id { get; set; }
    public string BlockId { get; set; }
    public string Hash { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid OwnerId { get; set; }
}