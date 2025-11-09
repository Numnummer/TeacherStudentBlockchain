using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Blockchain;
using WebApplication1.Models.Services;

namespace WebApplication1.Controllers;

public class Teacher(IBlockchainService blockchainService,
    BlockSystemService blockSystemService) : Controller
{
    // GET
    public IActionResult Index(string resource)
    {
        return View(model: resource);
    }

    public async Task<IActionResult> Blocks(string resource)
    {
        var res= await blockchainService.GetBlocks(resource);
        return View(res);
    }
    
    public async Task<IActionResult> GetFile(string blockId)
    {
        var block = await blockSystemService.GetBlockByIdAsync(blockId);
        return File(
            fileContents: block.FileContent,
            contentType: "application/octet-stream",
            fileDownloadName: block.FilePath
        );
    }
}