using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Authorize(Roles = "Teacher, Student")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("Teacher"))
        {
            var resources = new Resource[]
            {
                new Resource()
                {
                    Subject = "Предмет1",
                    Groups = ["Группа1","Группа2","Группа3"]
                },
                new Resource()
                {
                    Subject = "Предмет2",
                    Groups = ["Группа1","Группа4","Группа5"]
                }
            };
            using var rsa = RSA.Create(2048);
            var userDisplayData = new UserDisplayData()
            {
                Resources = resources,
                Role = "Teacher",
                PublicKey = rsa.ExportParameters(false),
                PrivateKey = rsa.ExportParameters(true)
            };
            return View(userDisplayData);
        }
        else
        {
            var resources = new Resource[]
            {
                new Resource()
                {
                    Subject = "Предмет1",
                    Groups = ["Группа1"]
                },
                new Resource()
                {
                    Subject = "Предмет2",
                    Groups = ["Группа1"]
                }
            };
            using var rsa = RSA.Create(2048);
            var userDisplayData = new UserDisplayData()
            {
                Resources = resources,
                Role = "Student",
                PublicKey = rsa.ExportParameters(false),
                PrivateKey = rsa.ExportParameters(true)
            };
            return View(userDisplayData);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}