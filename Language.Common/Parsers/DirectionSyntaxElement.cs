using Language.Api.Syntax;

namespace Language.Common.Parsers;

public class DirectionSyntaxElement : SyntaxElement
{
    public PlantDirection Direction { get; set; }
}

public enum PlantDirection: byte
{
    LeftToRight = 1,
    TopToBottom = 2
}