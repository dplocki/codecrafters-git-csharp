using System.IO.Compression;
using System.Text;

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

var command = args[0];

if (command == "init")
{
    Directory.CreateDirectory(".git");
    Directory.CreateDirectory(".git/objects");
    Directory.CreateDirectory(".git/refs");
    File.WriteAllText(".git/HEAD", "ref: refs/heads/main\n");
    Console.WriteLine("Initialized git directory");
}
else if (command == "cat-file")
{
    var localArgs = args.Skip(1).ToArray();

    if (localArgs.Count() < 1 ) {
        Console.WriteLine("Please provide a sub-command.");
        return;
    }

    if (localArgs[0] != "-p") {
        throw new ArgumentException($"Unknown sub-command {localArgs[0]}");
    }

    if (localArgs.Length < 2 ) {
        Console.WriteLine("Please provide a blob hash.");
        return;
    }

    var hash = localArgs[1];
    if (hash.Length != 40) {
        throw new ArgumentException($"Unk");
    }

    var filePath = $".git/objects/{hash.Substring(0, 2)}/{hash.Substring(2)}";
    using var compressedFileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    using var decompressStream = new ZLibStream(compressedFileStream, CompressionMode.Decompress);
    using var binaryStream = new BinaryReader(decompressStream);

    using var memoryStream = new MemoryStream();
    decompressStream.CopyTo(memoryStream);
    var data = memoryStream.ToArray();
    var nullIndex = Array.IndexOf(data, (byte)0);
    var header = Encoding.UTF8.GetString(data, 0, nullIndex);

    var content = new byte[data.Length - nullIndex - 1];
    Array.Copy(data, nullIndex + 1, content, 0, content.Length);

    string contentString = Encoding.UTF8.GetString(content);
    Console.Write(contentString);
}
else
{
    throw new ArgumentException($"Unknown command {command}");
}
