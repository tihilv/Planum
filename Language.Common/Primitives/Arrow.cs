using Language.Common.Utils;

namespace Language.Common.Primitives;

public struct Arrow
{
    public readonly ArrowShape Start;
    public readonly ArrowShape End;
    public readonly byte Length;
    public readonly LineType Type;
    public readonly Direction Direction;

    public Arrow(ArrowShape start, ArrowShape end, Byte length, LineType type = LineType.Plain, Direction direction = Direction.Undefined)
    {
        Start = start;
        End = end;
        Length = length;
        Type = type;
        Direction = direction;
    }
}

public class ArrowParser
{
    public static readonly ArrowParser Instance = new ArrowParser();

    private readonly (string, ArrowShape)[] _startShapes;
    private readonly (string, ArrowShape)[] _endShapes;
    private readonly Dictionary<char, LineType> _lineTypeBySymbol;
    private readonly (string, Direction)[] _directions;
    
    private ArrowParser()
    {
        _startShapes = new (String, ArrowShape)[]
        {
            ("<|", ArrowShape.Triangle),
            ("<", ArrowShape.Arrow),
            ("^", ArrowShape.Triangle),
            ("*", ArrowShape.DiamondFull),
            ("o", ArrowShape.DiamondEmpty),
            ("+", ArrowShape.Cross),
            ("#", ArrowShape.Rectangle),
            ("<<", ArrowShape.LittleTriangle),
            ("0)", ArrowShape.Connect),
            ("0", ArrowShape.Circle)
        };
        
        _endShapes = new (String, ArrowShape)[]
        {
            ("|>", ArrowShape.Triangle),
            (">", ArrowShape.Arrow),
            ("^", ArrowShape.Triangle),
            ("*", ArrowShape.DiamondFull),
            ("o", ArrowShape.DiamondEmpty),
            ("+", ArrowShape.Cross),
            ("#", ArrowShape.Rectangle),
            (">>", ArrowShape.LittleTriangle),
            ("0)", ArrowShape.Connect),
            ("0", ArrowShape.Circle)
        };
        
        _directions = new (string, Direction)[]
        {
            ("left", Direction.Left),
            ("right", Direction.Right),
            ("up", Direction.Up),
            ("down", Direction.Down),
            ("l", Direction.Left),
            ("r", Direction.Right),
            ("u", Direction.Up),
            ("d", Direction.Down)
        };

        _lineTypeBySymbol = new Dictionary<char, LineType>
        {
            { '-', LineType.Plain },
            { '.', LineType.Dash },
            { '~', LineType.Dot },
            { '=', LineType.Bold }
        };
        
        
    }

    public Arrow? Parse(string str)
    {
        int index = 0;
        var startShape = FindShape(str, _startShapes, ref index);
        ArrowShape endShape = ArrowShape.No;
        var lineSymbol = str[index];
        if (!_lineTypeBySymbol.TryGetValue(lineSymbol, out var lineType))
            return null;

        byte length = 0;
        Direction direction = Direction.Undefined;
        for (; index < str.Length; index++)
        {
            if (str[index] == lineSymbol)
            {
                length++;
                continue;
            }

            if (direction == Direction.Undefined)
            {
                direction = FindDirection(str, ref index);
                if (direction != Direction.Undefined)
                    continue;
            }

            if (endShape == ArrowShape.No)
            {
                endShape = FindShape(str, _endShapes, ref index);
                if (endShape != ArrowShape.No)
                    continue;
            }

            return null;
        }
        
        return new Arrow(startShape, endShape, length, lineType, direction);
    }

    private Direction FindDirection(String str, ref int index)
    {
        foreach (var pair in _directions)
        {
            if (StringMatcher.Match(str, index, pair.Item1))
            {
                index += pair.Item1.Length - 1;
                return pair.Item2;
            }
        }

        return Direction.Undefined;
    }

    private ArrowShape FindShape(string str, (string, ArrowShape)[] shapes, ref int index)
    {
        foreach (var shape in shapes)
        {
            if (StringMatcher.Match(str, index, shape.Item1))
            {
                index += shape.Item1.Length;
                return shape.Item2;
            }
        }

        return ArrowShape.No;
    }
    
    
}