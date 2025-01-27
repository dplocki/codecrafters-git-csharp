using System.Reflection;

class SubProgramsFactory()
{
    private readonly Dictionary<string, Type> subPrograms = [];

    public void Register(Type type)
    {
        var attribute = type.GetCustomAttribute<SubProgramAttribute>() ?? throw new InvalidOperationException("missing the name attrib");
        subPrograms.Add(attribute.Name, type);
    }

    public Type? Get(string command)
    {
        if (subPrograms.TryGetValue(command, out Type? value)) {
            return value;
        }

        return null;
    }
}
