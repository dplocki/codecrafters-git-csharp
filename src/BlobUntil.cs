using System.IO.Compression;
using System.Security.Cryptography;

internal static class BlobUntil
{
    public static string GetPathForHash(string hash)
    {
        return Path.Combine([
                ".git",
                "objects",
                hash[..2],
                hash[2..]
            ]);
    }

    public static void CreateDirectoryForHash(string hash)
    {
        if (!Directory.Exists($".git/objects/{hash[..2]}"))
        {
            Directory.CreateDirectory($".git/objects/{hash[..2]}");
        }
    }

    public static MemoryStream DecompressBlob(string hash)
    {
        var filePath = GetPathForHash(hash);
        using var compressedFileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var decompressStream = new ZLibStream(compressedFileStream, CompressionMode.Decompress);
        using var binaryStream = new BinaryReader(decompressStream);
        var memoryStream = new MemoryStream();

        decompressStream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public static string WriteBlob(MemoryStream memoryStream)
    {
        memoryStream.Position = 0;

        var hash = CalculateHash(memoryStream);

        CreateDirectoryForHash(hash);

        using var outputFileStream = File.Create(GetPathForHash(hash));
        using var compressStream = new ZLibStream(outputFileStream, CompressionMode.Compress);

        memoryStream.CopyTo(compressStream);

        return hash;
    }

    private static string CalculateHash(Stream memoryStream)
    {
        using var sha1 = SHA1.Create();

        memoryStream.Position = 0;
        var hashBytes = sha1.ComputeHash(memoryStream);
        return Convert.ToHexStringLower(hashBytes);
    }
}