
[SubProgram("init")]
internal class InitSubProgram : ISubProgram
{
    public Task<int> Run(string[] args)
    {
        Directory.CreateDirectory(".git");
        Directory.CreateDirectory(".git/objects");
        Directory.CreateDirectory(".git/refs");
        File.WriteAllText(".git/HEAD", "ref: refs/heads/main\n");
        Console.WriteLine("Initialized git directory");

        return Task.FromResult(0);
    }
}