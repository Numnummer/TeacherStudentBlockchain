using System.Security.Cryptography;

namespace WebApplication1.Models;

public class UserDisplayData
{
    public UserDisplayData(Resource[] resources, string role, string selectedResource, RSAParameters publicKey, RSAParameters privateKey)
    {
        Id=Guid.NewGuid();
        Resources = resources;
        Role = role;
        SelectedResource = selectedResource;
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public UserDisplayData()
    {
        
    }

    public Guid Id { get; set; }
    public Resource[] Resources { get; set; }
    public string Role { get; set; }
    public string SelectedResource { get; set; }
    public RSAParameters PublicKey { get; set; }
    public string PublicKeySerialized { get; set; }
    public RSAParameters PrivateKey { get; set; }
    public string PrivateKeySerialized { get; set; }
    public IFormFile File { get; set; }
    public string CaptchaAnswer { get; set; }
    public bool IsCaptchaInvalid { get; set; }
}