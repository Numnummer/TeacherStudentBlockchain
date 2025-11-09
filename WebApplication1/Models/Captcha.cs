namespace WebApplication1.Models;

public class Captcha
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public string? Problem { get; set; }
    public string? Answer { get; set; }
    public IFormFile? UserFile { get; set; }
    public byte[] FileContent { get; set; }
}