using System.Text;

internal class HashObjectSubProgram
{
    public static void Run(string inputFilePath)
    {
        using var memoryStream = new MemoryStream();
        using var inputFileStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        var additionalBytes = Encoding.UTF8.GetBytes($"blob {inputFileStream.Length}\0");

        memoryStream.Write(additionalBytes, 0, additionalBytes.Length);
        inputFileStream.CopyTo(memoryStream);

        Console.WriteLine(BlobUntil.WriteBlob(memoryStream));
    }
}
