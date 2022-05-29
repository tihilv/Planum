using Language.Api;

namespace Language.Common.Parsers;

public class DirectionParser : IParser
{
    public static readonly IParser Instance = new DirectionParser();

    public static string[] LeftToRightDirection = new[] { "left", "to", "right", "direction" };
    public static string[] TopToBottomDirection = new[] { "top", "to", "bottom", "direction" };
    
    private DirectionParser()
    {
        
    }

    public ParseResult? Parse(Token[] tokens)
    {
        if (tokens.Length == 4)
        {
            if (Match(tokens, LeftToRightDirection))
                return new ParseResult(new DirectionSyntaxElement(){Direction = PlantDirection.LeftToRight});
            
            if (Match(tokens, TopToBottomDirection))
                return new ParseResult(new DirectionSyntaxElement(){Direction = PlantDirection.TopToBottom});
        }
        
        return null;
    }


    private bool Match(IReadOnlyCollection<Token> tokens, string[] strings)
    {
        int index = 0;
        foreach (var token in tokens)
        {
            if (!token.Value.Equals(LeftToRightDirection[index]))
                return false;
            
            index++;
        }

        return index == strings.Length;
    }
}