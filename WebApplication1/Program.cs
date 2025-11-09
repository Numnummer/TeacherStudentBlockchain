using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication1.Models;
using WebApplication1.Models.Blockchain;
using WebApplication1.Models.Options;
using WebApplication1.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IBlockchainService, BlockchainService>();
builder.Services.AddSingleton<ICaptchaService, CaptchaService>();
builder.Services.AddSingleton<BlockSystemService>();
builder.Services.Configure<ResourcesOptions>(builder.Configuration.GetSection("Resources"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();