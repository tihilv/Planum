using Language.Api.Syntax;

namespace Bundle.Uml.Elements;

public class UmlContainerSyntaxElement : CompositeSyntaxElement, IGroupSyntaxElement
{
    public readonly UmlContainerType Type;
    public readonly string Name;
    public readonly string? Url;

    public UmlContainerSyntaxElement(UmlContainerType type, String name, String? url = null)
    {
        Type = type;
        Name = name;
        Url = url;
    }

    public UmlContainerSyntaxElement With(string? url = null, string? name = null)
    {
        return With(this, new UmlContainerSyntaxElement(Type, name??Name, url??Url));
    }
}

