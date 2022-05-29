using Language.Api;
using Language.Common.Elements;

namespace Language.Common.Parsers;

public class CommentsParser : IParser
{
    public static readonly IParser Instance = new CommentsParser();
    private CommentsParser()
    {
        
    }

    public ParseResult? Parse(Token[] tokens)
    {
        var firstToken = tokens.FirstOrDefault();
        if (firstToken?.Value.StartsWith("'") == true)
            return new ParseResult(new CommentsSyntaxElement(firstToken.ScriptLine.Value));
        
        return null;
    }
}