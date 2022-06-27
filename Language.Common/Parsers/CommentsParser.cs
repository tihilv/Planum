using Language.Api;
using Language.Api.Syntax;
using Language.Common.Elements;

namespace Language.Common.Parsers;

public class CommentsParser : IParser
{
    private const string CommentToken = "'";
    
    public static readonly IParser Instance = new CommentsParser();
    private CommentsParser()
    {
        
    }

    public ParseResult? Parse(Token[] tokens)
    {
        var firstToken = tokens.FirstOrDefault();
        if (firstToken?.Value.StartsWith(CommentToken) == true)
            return new ParseResult(new CommentsSyntaxElement(firstToken.ScriptLine.Value));
        
        return null;
    }

    public SynthesizeResult? Synthesize(SyntaxElement element)
    {
        if (element is CommentsSyntaxElement el)
            return new SynthesizeResult($"{CommentToken} {el.Value}");

        return null;
    }
}