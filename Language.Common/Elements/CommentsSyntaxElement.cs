using Language.Api.Syntax;

namespace Language.Common.Elements;

public class CommentsSyntaxElement : SyntaxElement
{
    public String Value { get; set; }

    public CommentsSyntaxElement(String scriptLineValue)
    {
        Value = scriptLineValue;
        throw new NotImplementedException();
    }
}