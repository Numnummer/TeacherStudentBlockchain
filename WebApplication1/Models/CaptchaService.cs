namespace WebApplication1.Models;

public class CaptchaService : ICaptchaService
{
    private readonly List<Captcha> _pendingCaptchaList = [];
    public Captcha GenerateCaptcha(Guid userId, IFormFile userFile)
    {
        var random = new Random();
        var a=random.Next(1, 1000);
        var b = random.Next(1, 1000);
        var answer = a + b;
        var captcha = new Captcha()
        {
            UserId = userId,
            SessionId = Guid.NewGuid(),
            Answer = answer.ToString(),
            Problem = $"{a} + {b} = ",
            UserFile = userFile
        };
        _pendingCaptchaList.Add(captcha);
        return captcha;
    }

    public bool ValidateCaptcha(string captchaResponse, Guid userId)
    {
        var captcha = _pendingCaptchaList.FirstOrDefault(x => x.UserId == userId);
        return captcha != null && captcha.Answer == captchaResponse;
    }

    public IFormFile? GetFile(Guid userId)
    {
        var captcha = _pendingCaptchaList.FirstOrDefault(x => x.UserId == userId);
        return captcha?.UserFile;
    }

    public void DeleteCaptcha(Guid userId)
    {
        var captcha = _pendingCaptchaList.FirstOrDefault(x => x.UserId == userId);
        if (captcha != null)
            _pendingCaptchaList.Remove(captcha);
    }
}