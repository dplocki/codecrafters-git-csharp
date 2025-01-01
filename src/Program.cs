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
else if (command == "hash-object")
{
    HashObjectSubProgram.Run(localArgs);
}
else if (command == "ls-tree")
{
    if (localArgs.Length == 0)
    {
        Console.WriteLine("Please provide a sub-command parameters.");
        return;
    }

    var nameOnly = localArgs.Contains("--name-only");
    if ((nameOnly && localArgs.Length == 1) || (!nameOnly && localArgs.Length == 0) ) {
        Console.WriteLine("Please provide a hash.");
        return;
    }

    var hash = localArgs.First(arg => arg != "--name-only");
    LsTreeSubProgram.Run(hash, nameOnly);
}
else
{
    throw new ArgumentException($"Unknown command {command}");
}
