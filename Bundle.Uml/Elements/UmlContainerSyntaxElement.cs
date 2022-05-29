using Language.Api.Syntax;

namespace Bundle.Uml.Elements;

public class UmlContainerSyntaxElement : CompositeSyntaxElement
{
    public readonly UmlContainerType Type;
    public readonly string Name;

    public UmlContainerSyntaxElement(UmlContainerType type, String name)
    {
        Type = type;
        Name = name;
    }
}