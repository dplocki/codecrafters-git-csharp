var subProgramFactory = new SubProgramsFactory();
subProgramFactory.Register(typeof(InitSubProgram));
subProgramFactory.Register(typeof(CatFileSubProgram));
subProgramFactory.Register(typeof(HashObjectSubProgram));
subProgramFactory.Register(typeof(LsTreeSubProgram));
subProgramFactory.Register(typeof(WriteTreeSubProgram));
subProgramFactory.Register(typeof(CloneSubProgram));

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return 1;
}

var command = args[0];
var localArgs = args.Skip(1).ToArray();
var commandType = subProgramFactory.Get(command);
if (commandType == null) {
    Console.Error.WriteLine($"Unknown command {command}");
    return 1;
}

var commandHandler = Activator.CreateInstance(commandType) as ISubProgram;
return await commandHandler!.Run(localArgs);
