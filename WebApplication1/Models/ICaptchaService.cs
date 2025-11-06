namespace WebApplication1.Models;

public interface ICaptchaService
{
    Captcha GenerateCaptcha(Guid userId, IFormFile userFile);
    bool ValidateCaptcha(string captchaResponse, Guid userId);
    IFormFile? GetFile(Guid userId);
    void DeleteCaptcha(Guid userId);
}