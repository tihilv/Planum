using System.Text;
using Language.Common.Utils;

namespace Language.Common.Primitives;

public struct Arrow : IEquatable<Arrow>
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

    public override String ToString()
    {
        return ArrowParser.Instance.Synthesize(this);
    }
    
#region Equality
    public bool Equals(Arrow other)
    {
        return Start == other.Start && End == other.End && Length == other.Length && Type == other.Type && Direction == other.Direction;
    }

    public override bool Equals(object? obj)
    {
        return obj is Arrow other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)Start;
            hashCode = (hashCode * 397) ^ (int)End;
            hashCode = (hashCode * 397) ^ Length.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Type;
            hashCode = (hashCode * 397) ^ (int)Direction;
            return hashCode;
        }
    }

    public static bool operator ==(Arrow left, Arrow right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Arrow left, Arrow right)
    {
        return !left.Equals(right);
    }
#endregion
}

public class ArrowParser
{
    public static readonly ArrowParser Instance = new ArrowParser();

    private readonly (string, ArrowShape)[] _startShapes;
    private readonly (string, ArrowShape)[] _endShapes;
    private readonly Dictionary<char, LineType> _lineTypeBySymbol;
    private readonly (string, Direction)[] _directions;

    private readonly Dictionary<ArrowShape, string> _reverseStartShapes;
    private readonly Dictionary<ArrowShape, string> _reverseEndShapes;
    private readonly Dictionary<LineType, char> _reverseLineTypeBySymbol;
    private readonly Dictionary<Direction, string> _reverseDirections;

    private ArrowParser()
    {
        _startShapes = new (String, ArrowShape)[]
        {
            ("^", ArrowShape.Triangle),
            ("<|", ArrowShape.Triangle),
            ("<", ArrowShape.Arrow),
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
            ("^", ArrowShape.Triangle),
            ("|>", ArrowShape.Triangle),
            (">", ArrowShape.Arrow),
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

        _reverseStartShapes = new Dictionary<ArrowShape, String>();
        foreach (var shape in _startShapes)
            _reverseStartShapes[shape.Item2] = shape.Item1;

        _reverseEndShapes = new Dictionary<ArrowShape, String>();
        foreach (var shape in _endShapes)
            _reverseEndShapes[shape.Item2] = shape.Item1;
        
        _reverseLineTypeBySymbol = new Dictionary<LineType, char>();
        foreach (var lt in _lineTypeBySymbol)
            _reverseLineTypeBySymbol.Add(lt.Value, lt.Key);
        
        _reverseDirections = new Dictionary<Direction, string>();
        foreach (var dir in _directions)
            _reverseDirections[dir.Item2] = dir.Item1;
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


    public String Synthesize(Arrow arrow)
    {
        var sb = new StringBuilder();
        
        if (arrow.Start != ArrowShape.No)
            sb.Append(_reverseStartShapes[arrow.Start]);

        var s = _reverseLineTypeBySymbol[arrow.Type];
        var middleIndex = Math.Max(0, arrow.Length / 2 - 1);

        for (int i = 0; i < arrow.Length; i++)
        {
            sb.Append(s);
            if (i == middleIndex && arrow.Direction != Direction.Undefined)
                sb.Append(_reverseDirections[arrow.Direction]);
        }

        if (arrow.End != ArrowShape.No)
            sb.Append(_reverseEndShapes[arrow.End]);

        return sb.ToString();
    }
}