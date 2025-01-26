[AttributeUsage(AttributeTargets.Class)]
public class SubProgramAttribute(string name) : Attribute
{
    public string Name { get; private set; } = name;
}
