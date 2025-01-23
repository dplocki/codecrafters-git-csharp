internal class CloneSubProgram
{
    public static async Task Run(string url, string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            Console.Error.WriteLine($"Path {directoryPath} already exist");
            return;
        }

        Directory.CreateDirectory(directoryPath);

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("git-protocol", "version=1");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");

        var serviceName = "git-upload-pack";
        var discoveryUrl = $"{url}/info/refs?service={serviceName}";
        var discoveryResponse = await httpClient.GetAsync(discoveryUrl);

        if (!discoveryResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {discoveryResponse.StatusCode}");
            Console.WriteLine(await discoveryResponse.Content.ReadAsStringAsync());
            return;
        }

        var discoveryContent = await discoveryResponse.Content.ReadAsStringAsync();

        Console.WriteLine(discoveryContent);
    }
}
