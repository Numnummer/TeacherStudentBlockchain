using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Services;

namespace WebApplication1.Controllers;

public class Teacher(IBlockchainService blockchainService) : Controller
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
}