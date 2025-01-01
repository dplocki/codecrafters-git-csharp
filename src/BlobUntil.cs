using System.IO.Compression;

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
}