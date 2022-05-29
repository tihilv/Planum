using Language.Api.Syntax;

namespace Language.Common.Elements;

public class IncludeSyntaxElement : SyntaxElement
{
    public string ResourceName { get; set; } = null!;
}