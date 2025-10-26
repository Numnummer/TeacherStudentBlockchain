using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Blockchain;

namespace WebApplication1.Controllers;

public class Student(IBlockchainService blockchainService) : Controller
{
    // GET
    public IActionResult Index(string userData)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true, 
            WriteIndented = true
        };
        var user=JsonSerializer.Deserialize<UserDisplayData>(userData, options);
        user.PublicKeySerialized = JsonSerializer.Serialize(user.PublicKey, options);
        user.PrivateKeySerialized = JsonSerializer.Serialize(user.PrivateKey, options);
        return View(user);
    }

    [HttpPost]
    public IActionResult Index(UserDisplayData user)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true, 
            WriteIndented = true
        };
        user.PublicKey=JsonSerializer.Deserialize<RSAParameters>(user.PublicKeySerialized, options);
        user.PrivateKey=JsonSerializer.Deserialize<RSAParameters>(user.PrivateKeySerialized, options);
        var file = user.File;
        blockchainService.AddBlock(user);
        return View(user);
    }
}