using Language.Api;
using Language.Api.Exceptions;
using Language.Common.Elements;

namespace Language.Common.Parsers;

public class IncludeParser : IParser
{
    public static readonly IParser Instance = new IncludeParser();
    private IncludeParser()
    {
        
    }

    public ParseResult? Parse(Token[] tokens)
    {
        var firstToken = tokens.FirstOrDefault();
        if (firstToken?.Value.Equals("!include") == true)
        {
            if (tokens.Length != 2)
                throw new ParsingException(firstToken.Range, "Incorrect include syntax");
            
            return new ParseResult(new IncludeSyntaxElement() { ResourceName = tokens.Last().Value });
        }

        return null;
    }
}