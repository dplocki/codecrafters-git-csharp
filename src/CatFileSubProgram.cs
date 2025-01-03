using System.Text;

internal static class CatFileSubProgram
{
    public static void Run(string hash)
    {
        using var memoryStream = BlobUntil.DecompressBlob(hash);
        var data = memoryStream.ToArray();
        var nullIndex = Array.IndexOf(data, (byte)0);
        var header = Encoding.UTF8.GetString(data, 0, nullIndex);

        var content = new byte[data.Length - nullIndex - 1];
        Array.Copy(data, nullIndex + 1, content, 0, content.Length);

        string contentString = Encoding.UTF8.GetString(content);
        Console.Write(contentString);
    }
}
