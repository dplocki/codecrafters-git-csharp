using System.Text;

struct TreeLine
{
    public string Mode;
    public string Type;
    public string Hash;
    public string Name;
}

internal class LsTreeSubProgram
{
    private const byte Null = 0;
    private const byte Space = 32;

    public static void Run(string hash, bool nameOnly)
    {
        using var memoryStream = BlobUntil.DecompressBlob(hash);

        var data = memoryStream.ToArray();
        var spaceIndex = Array.IndexOf(data, Space);
        var nullIndex = Array.IndexOf(data, Null);
        var contentSize = int.Parse(Encoding.UTF8.GetString(data, spaceIndex, nullIndex - spaceIndex));

        foreach (var item in ParseTreeEntries(data, nullIndex + 1))
        {
            if (nameOnly)
            {
                Console.WriteLine(item.Name);
            }
            else
            {
                Console.WriteLine($"{item.Mode} {item.Type} {item.Hash} {item.Name}");
            }
        }
    }

    private static IEnumerable<TreeLine> ParseTreeEntries(byte[] data, int tokenBegin)
    {
        do
        {
            int tokenEnd = Array.IndexOf(data, Space, tokenBegin);
            var mode = Encoding.UTF8.GetString(data, tokenBegin, tokenEnd - tokenBegin);
            tokenBegin = tokenEnd + 1;

            var type = (mode == "40000") ? "tree" : "blob";

            tokenEnd = Array.IndexOf(data, Null, tokenBegin);
            var name = Encoding.UTF8.GetString(data, tokenBegin, tokenEnd - tokenBegin);
            tokenBegin = tokenEnd + 1;

            var hash = new byte[20];
            Array.Copy(data, tokenBegin, hash, 0, 20);
            tokenBegin += 20;

            yield return new TreeLine
            {
                Mode = mode,
                Type = type,
                Hash = Convert.ToHexStringLower(hash),
                Name = name
            };
        } while (tokenBegin < data.Length);
    }
}
