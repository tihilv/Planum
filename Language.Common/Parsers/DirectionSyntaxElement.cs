using Language.Api.Syntax;

namespace Language.Common.Parsers;

public class DirectionSyntaxElement : SyntaxElement
{
    public readonly PlantDirection Direction;

    public DirectionSyntaxElement(PlantDirection direction)
    {
        Direction = direction;
    }
}

public enum PlantDirection: byte
{
    LeftToRight = 1,
    TopToBottom = 2
}