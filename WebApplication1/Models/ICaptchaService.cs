namespace WebApplication1.Models;

public interface ICaptchaService
{
    Captcha GenerateCaptcha(Guid userId, IFormFile userFile, byte[] fileContent);
    bool ValidateCaptcha(string captchaResponse, Guid userId);
    IFormFile? GetFile(Guid userId);
    void DeleteCaptcha(Guid userId);
    byte[]? GetFileContent(Guid userId);
}