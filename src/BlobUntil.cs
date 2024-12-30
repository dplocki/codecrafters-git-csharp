internal static class BlobUntil
{
    public static string GetPathForHash(string hash)
    {
        return Path.Combine([
                ".git",
                "objects",
                hash[..2],
                hash[2..]
            ]);
    }

    public static void CreateDirectoryForHash(string hash)
    {
        if (!Directory.Exists($".git/objects/{hash[..2]}"))
        {
            Directory.CreateDirectory($".git/objects/{hash[..2]}");
        }
    }
}