using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("Account/Login")]
public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(LoginForm loginForm)
    {
        switch (loginForm.Username)
        {
            case "teacher":
                await SignInAsync(loginForm.Username, "Teacher");
                break;
            case "student":
                await SignInAsync(loginForm.Username, "Student");
                break;
        }

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login", new { area = "" });
    }

    private async Task SignInAsync(string username, string role)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, username),
            new (ClaimTypes.Role, role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}