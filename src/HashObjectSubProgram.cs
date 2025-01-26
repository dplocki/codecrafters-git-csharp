
[SubProgram("hash-object")]
internal class HashObjectSubProgram
{
    public static void Run(string inputFilePath)
    {
        Console.WriteLine(BlobUntil.SaveFileAsBlob(inputFilePath));
    }
}
