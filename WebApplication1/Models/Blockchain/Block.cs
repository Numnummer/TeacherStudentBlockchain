using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Models.Blockchain;

public class Block
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonRepresentation(BsonType.String)]
    public Guid BlockId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? PreviousHash { get; set; }
    public string FilePath { get; set; }
    
    [BsonRepresentation(BsonType.Binary)]
    public byte[] DataSignature { get; set; }
    
    [BsonRepresentation(BsonType.Binary)]
    public byte[] HashSignature { get; set; }
    public ulong Nonce { get; set; }

    public Block(string filePath, string? previousHash, Guid ownerId)
    {
        BlockId = Guid.NewGuid();
        Nonce = 0;
        FilePath = filePath;
        PreviousHash = previousHash;
        OwnerId = ownerId;
    }

    public void SignBlock(RSAParameters rsaParameters)
    {
        using var rsa = RSA.Create();
        using var sha256 = SHA256.Create();
        
        rsa.ImportParameters(rsaParameters);
        var data = Encoding.UTF8.GetBytes(FilePath);
        DataSignature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var hash = GetHash(Nonce);
        HashSignature = rsa.SignData(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public bool ValidateBlock(RSAParameters rsaParameters)
    {
        using var rsa = RSA.Create();
        rsa.ImportParameters(rsaParameters);
        var data = Encoding.UTF8.GetBytes(FilePath);
        var hash = GetHash(Nonce);
        return rsa.VerifyData(data, DataSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)
            && rsa.VerifyData(hash, HashSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public byte[] GetHash(ulong nonce)
    {
        using var sha256 = SHA256.Create();
        var data = Encoding.UTF8.GetBytes(FilePath + nonce.ToString());
        return sha256.ComputeHash(data);
    }
    
    public static string BytesToHex(byte[] bytes)
    {
        var hex = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }

    public void MineBlock(byte difficulty, RSAParameters rsaParameters)
    {
        var target = new string('0', difficulty);
        ulong nonce = 0;
        var hash = BytesToHex(GetHash(nonce));
        do
        {
            nonce++;
            hash = BytesToHex(GetHash(nonce));
        } while (!hash.StartsWith(target));
        Nonce = nonce;
        SignBlock(rsaParameters);
        CreatedAt=DateTime.Now;
    }
}