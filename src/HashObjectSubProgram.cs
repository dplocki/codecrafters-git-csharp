
[SubProgram("hash-object")]
internal class HashObjectSubProgram : ISubProgram
{
    public Task<int> Run(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Please provide a sub-command parameters.");
            return Task.FromResult(1);
        }

        if (args[0] != "-w")
        {
            Console.Error.WriteLine($"Unknown sub-command parameter {args[0]}");
            return Task.FromResult(1);
        }

        if (args.Length < 2)
        {
            Console.Error.WriteLine("Please provide a file.");
            return Task.FromResult(1);
        }

        var inputFilePath = args[1];
        Console.WriteLine(BlobUntil.SaveFileAsBlob(inputFilePath));
        return Task.FromResult(0);
    }
}
