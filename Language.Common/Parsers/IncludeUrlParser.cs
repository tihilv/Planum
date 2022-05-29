using Language.Api;
using Language.Api.Exceptions;
using Language.Common.Elements;

namespace Language.Common.Parsers;

public class IncludeUrlParser : IParser
{
    public static readonly IParser Instance = new IncludeUrlParser();
    private IncludeUrlParser()
    {
        
    }

    public ParseResult? Parse(Token[] tokens)
    {
        var firstToken = tokens.FirstOrDefault();
        if (firstToken?.Value.Equals("!includeurl") == true)
        {
            if (tokens.Length != 2)
                throw new ParsingException(firstToken.Range, "Incorrect includeurl syntax");
            
            return new ParseResult(new IncludeUrlSyntaxElement() { Url = tokens.Last().Value });
        }

        return null;
    }
}