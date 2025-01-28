using System.Reflection;

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return 1;
}

var command = args[0];
var commandType = Assembly.GetExecutingAssembly()
    .GetTypes()
    .FirstOrDefault(type => {
       var attribute = type.GetCustomAttribute<SubProgramAttribute>();
       if (attribute == null)
       {
            return false;
       }

       return attribute.Name == command;
    });

if (commandType == null) {
    Console.Error.WriteLine($"Unknown command {command}");
    return 1;
}

var localArgs = args.Skip(1).ToArray();
var commandHandler = Activator.CreateInstance(commandType) as ISubProgram;
return await commandHandler!.Run(localArgs);
