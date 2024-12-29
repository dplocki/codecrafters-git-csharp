if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

var command = args[0];
var localArgs = args.Skip(1).ToArray();

if (command == "init")
{
    InitSubProgram.Run(localArgs);
}
else if (command == "cat-file")
{
    CatFileSubProgram.Run(localArgs);
}
else
{
    throw new ArgumentException($"Unknown command {command}");
}
