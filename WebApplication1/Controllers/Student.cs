using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WebApplication1.Models;
using WebApplication1.Models.Blockchain;
using WebApplication1.Models.Services;

namespace WebApplication1.Controllers;

public class Student(IBlockchainService blockchainService,
    ICaptchaService captchaService) : Controller
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
        /*if (!captchaService.ValidateCaptcha(user.CaptchaAnswer, user.Id))
        {
            user.IsCaptchaInvalid = true;
            return View(user);
        }*/
        user.IsCaptchaInvalid = false;
        user.File=captchaService.GetFile(user.Id);
        captchaService.DeleteCaptcha(user.Id);
        if (user.File == null)
            return StatusCode(500);
        var options = new JsonSerializerOptions
        {
            IncludeFields = true, 
            WriteIndented = true
        };
        user.PublicKey=JsonSerializer.Deserialize<RSAParameters>(user.PublicKeySerialized, options);
        user.PrivateKey=JsonSerializer.Deserialize<RSAParameters>(user.PrivateKeySerialized, options);
        blockchainService.AddBlock(user);
        return View(user);
    }

    [HttpPost]
    public IActionResult Captcha(UserDisplayData user)
    {
        using Image<Rgba32> image = new(600, 400);
        image.Mutate(img=>img.BackgroundColor(Color.White));
        var collection=new FontCollection();
        var family = collection.Add("wwwroot/Super Joyful.ttf");
        var font=family.CreateFont(42);
        var captcha = captchaService.GenerateCaptcha(user.Id, user.File);
        image.Mutate(x=> x.DrawText(captcha.Problem ?? "Error", font, Color.Black, new PointF(200, 90)));
        image.Save("./wwwroot/captcha.png");
        return View(user);
    }
}