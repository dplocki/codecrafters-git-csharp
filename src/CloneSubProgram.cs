internal class CloneSubProgram
{
    public static void Run(string link, string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            Console.Error.WriteLine($"Path {directoryPath} already exist");
            return;
        }

        Directory.CreateDirectory(directoryPath);
    }
}
