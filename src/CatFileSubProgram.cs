using System.Text;

[SubProgram("cat-file")]
internal class CatFileSubProgram : ISubProgram
{
    public Task<int> Run(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Please provide a sub-command parameters.");
            return Task.FromResult(1);
        }

        if (args[0] != "-p")
        {
            Console.Error.WriteLine($"Unknown sub-command parameter: {args[0]}");
            return Task.FromResult(1);
        }

        if (args.Length < 2)
        {
            Console.Error.WriteLine("Please provide a blob hash.");
            return Task.FromResult(1);
        }

        var hash = args[1];
        if (hash.Length != 40)
        {
            Console.Error.WriteLine("Provided hash is incorrect");
            return Task.FromResult(1);
        }

        using var memoryStream = BlobUntil.DecompressBlob(hash);
        var data = memoryStream.ToArray();
        var nullIndex = Array.IndexOf(data, (byte)0);
        var header = Encoding.UTF8.GetString(data, 0, nullIndex);

        var content = new byte[data.Length - nullIndex - 1];
        Array.Copy(data, nullIndex + 1, content, 0, content.Length);

        string contentString = Encoding.UTF8.GetString(content);
        Console.Write(contentString);

        return Task.FromResult(0);
    }
}
