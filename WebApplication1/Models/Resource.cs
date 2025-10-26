using WebApplication1.Models.Blockchain;

namespace WebApplication1.Models;

public class Resource
{
    public string? Subject { get; set; }
    public string[]? Groups { get; set; }
    public FilesBlockchain FilesBlockchain { get; set; }
}