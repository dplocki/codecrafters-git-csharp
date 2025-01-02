if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

var command = args[0];
var localArgs = args.Skip(1).ToArray();

switch (command)
{
    case "init":
        InitSubProgram.Run(localArgs);
        return;

    case "cat-file":
        CatFileSubProgram.Run(localArgs);
        return;

    case "hash-object":
        HashObjectSubProgram.Run(localArgs);
        return;

    case "ls-tree":
        if (localArgs.Length == 0)
        {
            Console.WriteLine("Please provide a sub-command parameters.");
            return;
        }

        var nameOnly = localArgs.Contains("--name-only");
        if (nameOnly && localArgs.Length == 1 || !nameOnly && localArgs.Length == 0)
        {
            Console.WriteLine("Please provide a hash.");
            return;
        }

        var hash = localArgs.First(arg => arg != "--name-only");
        LsTreeSubProgram.Run(hash, nameOnly);
        return;

    case "write-tree":
        WriteTreeSubProgram.Run(localArgs);
        return;

    default:
        throw new ArgumentException($"Unknown command {command}");
}
