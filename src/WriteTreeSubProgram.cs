using System.Text;


[SubProgram("write-tree")]
internal class WriteTreeSubProgram : ISubProgram
{
    public Task<int> Run(string[] args)
    {
        var directoryPath = Directory.GetCurrentDirectory();
        Console.WriteLine(IterateThroughDirectory(directoryPath).ToString());
        return Task.FromResult(0);
    }

    private static Hash IterateThroughDirectory(string directoryPath)
    {
        var entries = Directory.EnumerateFileSystemEntries(directoryPath).Order();

        var results = entries
            .Select(fullPath => new FileInfo(fullPath))
            .Where(entryInfo => entryInfo.Name != ".git")
            .Select(entryInfo =>
            {
                if (entryInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return new TreeLine()
                    {
                        Mode = "40000",
                        Type = "tree",
                        Name = entryInfo.Name,
                        Hash = IterateThroughDirectory(entryInfo.FullName)
                    };
                }

                return new TreeLine()
                {
                    Mode = "100644",
                    Type = "blob",
                    Name = entryInfo.Name,
                    Hash = BlobUntil.SaveFileAsBlob(entryInfo.FullName)
                };
            })
            .ToArray();

        var size = results.Sum(entryInfo => entryInfo.Mode.Length + 1 + entryInfo.Name.Length + 1 + Hash.Length);
        var memorySteam = new MemoryStream();

        memorySteam.Write(Encoding.UTF8.GetBytes($"tree {size}\0"));

        foreach (var result in results)
        {
            memorySteam.Write(Encoding.UTF8.GetBytes($"{result.Mode} {result.Name}\0"));
            memorySteam.Write(result.Hash.Content, 0, Hash.Length);
        }

        return BlobUntil.WriteBlob(memorySteam);
    }
}
