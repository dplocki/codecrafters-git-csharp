[AttributeUsage(AttributeTargets.Class)]
public class SubProgramAttribute : Attribute
{
    public string Name { get; private set; }

    public SubProgramAttribute(string name)
    {
        Name = name;
    }
}