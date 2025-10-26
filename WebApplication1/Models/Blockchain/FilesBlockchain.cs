using System.Security.Cryptography;

namespace WebApplication1.Models.Blockchain;

public class FilesBlockchain
{
    private readonly List<Block> _blocks = [];
    private const byte Difficulty = 5;
    private readonly Dictionary<Guid, RSAParameters> _publicKeys = new Dictionary<Guid, RSAParameters>();

    public bool CreateGenesisBlock()
    {
        var block=new Block("Genesis Block", null, Guid.NewGuid());
        using var rsa = RSA.Create(2048);
        block.MineBlock(Difficulty, rsa.ExportParameters(true));
        if (!block.ValidateBlock(rsa.ExportParameters(false))) return false;
        _blocks.Add(block);
        return true;
    }
    
    public Block LastBlock => _blocks[^1];

    public bool AddBlock(Guid userId, string filePath, RSAParameters rsaPublicParameters, RSAParameters rsaPrivateParameters)
    {
        var prevHash = Block.BytesToHex(LastBlock.GetHash(LastBlock.Nonce));
        var block=new Block(filePath, prevHash, userId);
        block.MineBlock(Difficulty, rsaPrivateParameters);
        if (!block.ValidateBlock(rsaPublicParameters)) return false;
        _blocks.Add(block);
        _publicKeys.TryAdd(userId, rsaPublicParameters);
        return true;
    }

    public bool Validate()
    {
        for (int i = 1; i < _blocks.Count; i++)
        {
            if(_blocks[i].PreviousHash != Block.BytesToHex(_blocks[i - 1].GetHash(_blocks[i-1].Nonce)))
                return false;
        }

        return _blocks.All(block => block.ValidateBlock(_publicKeys[block.OwnerId]));
    }

    public BlockDisplayInfo[] GetBlocks()
    {
        return _blocks.Select(b => new BlockDisplayInfo()
        {
            OwnerId = b.OwnerId,
            Hash = Block.BytesToHex(b.GetHash(b.Nonce)),
            Id = b.Id,
            CreatedAt = b.CreatedAt,
            FilePath = b.FilePath
        }).ToArray();
    }
}