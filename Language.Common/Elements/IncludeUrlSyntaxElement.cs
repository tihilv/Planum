using Language.Api.Syntax;

namespace Language.Common.Elements;

public class IncludeUrlSyntaxElement : SyntaxElement
{
    public string Url { get; set; } = null!;
}