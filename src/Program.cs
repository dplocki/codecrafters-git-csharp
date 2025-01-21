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
        {
            InitSubProgram.Run(localArgs);
            return;
        }

    case "cat-file":
        {
            if (localArgs.Length < 1)
            {
                Console.Error.WriteLine("Please provide a sub-command parameters.");
                return;
            }

            if (localArgs[0] != "-p")
            {
                Console.Error.WriteLine($"Unknown sub-command parameter: {args[0]}");
                return;
            }

            if (localArgs.Length < 2)
            {
                Console.Error.WriteLine("Please provide a blob hash.");
                return;
            }

            var hash = localArgs[1];
            if (hash.Length != 40)
            {
                Console.Error.WriteLine("Provided hash is incorrect");
                return;
            }

            CatFileSubProgram.Run(hash);
            return;
        }

    case "hash-object":
        {
            if (localArgs.Length < 1)
            {
                Console.Error.WriteLine("Please provide a sub-command parameters.");
                return;
            }

            if (localArgs[0] != "-w")
            {
                Console.Error.WriteLine($"Unknown sub-command parameter {localArgs[0]}");
                return;
            }

            if (localArgs.Length < 2)
            {
                Console.Error.WriteLine("Please provide a file.");
                return;
            }

            var inputFilePath = localArgs[1];

            HashObjectSubProgram.Run(inputFilePath);
            return;
        }

    case "ls-tree":
        {
            if (localArgs.Length == 0)
            {
                Console.Error.WriteLine("Please provide a sub-command parameters.");
                return;
            }

            var nameOnly = localArgs.Contains("--name-only");
            if (nameOnly && localArgs.Length == 1 || !nameOnly && localArgs.Length == 0)
            {
                Console.Error.WriteLine("Please provide a hash.");
                return;
            }

            var hash = localArgs.First(arg => arg != "--name-only");
            LsTreeSubProgram.Run(hash, nameOnly);
            return;
        }

    case "write-tree":
        {
            WriteTreeSubProgram.Run();
            return;
        }

    case "clone":
        {
            if (localArgs.Length < 2)
            {
                Console.Error.WriteLine("Please provide a sub-command parameters");
                return;
            }

            var link = localArgs[0];
            var directoryPath = localArgs[1];

            CloneSubProgram.Run(link, directoryPath);
            return;
        }
}

Console.Error.WriteLine($"Unknown command {command}");
return;
