using System.Security.Cryptography;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace WebApplication1.Models.Blockchain;

public class FilesBlockchain
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public List<Block> Blocks { get; set; } = [];
    private const byte Difficulty = 5;
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
    private readonly Dictionary<string, RSAParameters> _publicKeys = new ();

    public bool CreateGenesisBlock()
    {
        var block=new Block("Genesis Block", null, Guid.NewGuid());
        using var rsa = RSA.Create(2048);
        block.MineBlock(Difficulty, rsa.ExportParameters(true));
        if (!block.ValidateBlock(rsa.ExportParameters(false))) return false;
        Blocks.Add(block);
        return true;
    }
    
    public Block? LastBlock => Blocks[^1];

    public bool AddBlock(Guid userId, string filePath, RSAParameters rsaPublicParameters, RSAParameters rsaPrivateParameters, byte[] fileContent)
    {
        var prevHash = Block.BytesToHex(LastBlock.GetHash(LastBlock.Nonce));
        var block=new Block(filePath, prevHash, userId);
        block.FileContent = fileContent;
        block.MineBlock(Difficulty, rsaPrivateParameters);
        if (!block.ValidateBlock(rsaPublicParameters)) return false;
        Blocks.Add(block);
        _publicKeys.TryAdd(userId.ToString(), rsaPublicParameters);
        return true;
    }

    public bool Validate()
    {
        for (int i = 1; i < Blocks.Count; i++)
        {
            if(Blocks[i].PreviousHash != Block.BytesToHex(Blocks[i - 1].GetHash(Blocks[i-1].Nonce)))
                return false;
        }

        return Blocks.All(block => block.ValidateBlock(_publicKeys[block.OwnerId.ToString()]));
    }

    public BlockDisplayInfo[] GetBlocks()
    {
        return Blocks.Select(b => new BlockDisplayInfo()
        {
            OwnerId = b.OwnerId,
            Hash = Block.BytesToHex(b.GetHash(b.Nonce)),
            Id = b.BlockId,
            CreatedAt = b.CreatedAt,
            FilePath = b.FilePath,
            BlockId = b.Id
        }).ToArray();
    }
}