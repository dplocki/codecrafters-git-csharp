using System.IO.Compression;
using System.Text;

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
        var directoryPath = Path.Combine(".git", "objects", hash[..2]);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
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

    public static Hash WriteBlob(MemoryStream memoryStream)
    {
        memoryStream.Position = 0;

        var hash = Hash.FromStream(memoryStream);
        var hashString = hash.ToString();

        CreateDirectoryForHash(hashString);

        using var outputFileStream = File.Create(GetPathForHash(hashString));
        using var compressStream = new ZLibStream(outputFileStream, CompressionMode.Compress);

        memoryStream.CopyTo(compressStream);

        return hash;
    }

    public static Hash SaveFileAsBlob(string inputFilePath)
    {
        using var memoryStream = new MemoryStream();
        using var inputFileStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        var additionalBytes = Encoding.UTF8.GetBytes($"blob {inputFileStream.Length}\0");

        memoryStream.Write(additionalBytes, 0, additionalBytes.Length);
        inputFileStream.CopyTo(memoryStream);

        return WriteBlob(memoryStream);
    }
}