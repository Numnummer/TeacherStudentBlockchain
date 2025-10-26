using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class Teacher(IBlockchainService blockchainService) : Controller
{
    // GET
    public IActionResult Index(string resource)
    {
        return View(model: resource);
    }

    public IActionResult Blocks(string resource)
    {
        var res=  blockchainService.GetBlocks(resource);
        return View(res);
    }
}