using System.Text;

[SubProgram("clone")]
internal class CloneSubProgram : ISubProgram
{
    private const string serviceName = "git-upload-pack";

    public async Task<int> Run(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Please provide a sub-command parameters");
            return 1;
        }

        var url = args[0];
        var directoryPath = args[1];

        if (Directory.Exists(directoryPath))
        {
            Console.Error.WriteLine($"Path {directoryPath} already exist");
            return 1;
        }

        Directory.CreateDirectory(directoryPath);

        var repositoryUrl = $"{url}/info/refs?service={serviceName}";

        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Accept", "application/x-git-upload-pack-advertisement");
        httpClient.DefaultRequestHeaders.Add("git-protocol", "version=1");

        using var stream = await httpClient.GetStreamAsync(repositoryUrl);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        foreach (var item in ParseGitRefs(await reader.ReadToEndAsync()))
        {
            Console.WriteLine(item.Key);
            Console.WriteLine(item.Value);
            Console.WriteLine();
            await DownloadGitObjectAsync(url, item.Value);
        }

        return 0;
    }

    private static Dictionary<string, string> ParseGitRefs(string response)
    {
        var refs = new Dictionary<string, string>();
        var lines = response.Split('\n');

        return lines.Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("0000"))
            .Where(line => line.Length > 4)
            .Select(line => line[4..].Trim().Split(' '))
            .ToDictionary(parts => parts[1], parts => parts[0]);
    }

    public static async Task<byte[]> DownloadGitObjectAsync(string url, string objectHash)
    {
        using var client = new HttpClient();

        var repositoryUrl = $"{url}/git/git-upload-pack";
        var payload = new StringContent(
            $"0032want {objectHash}\n" +
            "0000",
            Encoding.UTF8,
            "application/x-git-upload-pack-request"
        );

        var response = await client.PostAsync(repositoryUrl, payload);
        return await response.Content.ReadAsByteArrayAsync();
    }
}
