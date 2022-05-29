using Bundle.Uml.Elements;
using Language.Api;
using Language.Common.Primitives;

namespace Bundle.Uml.Parsers;

public class UmlElementParser : IParser
{
    public static readonly IParser Instance = new UmlElementParser();

    private readonly (char, char, UmlFigureType)[] _bracesAsFigureTypes;
    
    private UmlElementParser()
    {
        _bracesAsFigureTypes = new[]
        {
            (':', ':', UmlFigureType.Actor),
            ('(', ')', UmlFigureType.UseCase),
            ('[', ']', UmlFigureType.Component),
        };
    }

    public ParseResult? Parse(Token[] tokens)
    {
        int index = 0;
        var firstFigure = GetFigure(tokens, ref index);
        if (firstFigure == null)
            return null;

        var arrow = GetArrow(tokens, ref index);
        UmlFigure? secondFigure = null;
        if (arrow != null)
        {
            secondFigure = GetFigure(tokens, ref index);
        }

        var comment = GetComment(tokens, ref index);

        return new ParseResult(new UmlSyntaxElement(firstFigure.Value, arrow, secondFigure, comment));
    }

    private UmlFigure? GetFigure(Token[] tokens, ref int index)
    {
        if (tokens.Length <= index)
            return null;

        UmlFigureType figureType = UmlFigureType.Actor;
        
        if (Enum.TryParse(typeof(UmlFigureType), tokens[index].Value, true, out var value))
        {
            figureType = (UmlFigureType)value;
            index++;
        }

        if (tokens.Length <= index)
            return null;

        var figureName = tokens[index++].Value;
        foreach (var tuple in _bracesAsFigureTypes)
        {
            if (figureName.StartsWith(tuple.Item1) && figureName.EndsWith(tuple.Item2))
            {
                figureType = tuple.Item3;
                figureName = figureName.Substring(1, figureName.Length - 2);
                break;
            }
        }

        string? alias = null;
        if (tokens.Length > index)
        {
            if (tokens[index].Value.Equals("as", StringComparison.InvariantCultureIgnoreCase))
            {
                index++;
                if (tokens.Length <= index)
                    return null;

                alias = tokens[index++].Value;
            }
        }

        string? stereotype = null;
        if (tokens.Length > index)
        {
            if (tokens[index].Value.StartsWith("<<") && tokens[index].Value.EndsWith(">>"))
                stereotype = tokens[index].Value.Substring(2, tokens[index].Value.Length - 4).Trim();
        }

        figureName = figureName.Trim('"');
        return new UmlFigure(figureType, figureName, stereotype, alias);
    }

    private Arrow? GetArrow(Token[] tokens, ref Int32 index)
    {
        if (tokens.Length <= index)
            return null;

        var arrow = ArrowParser.Instance.Parse(tokens[index].Value);
        if (arrow != null)
            index++;

        return arrow;
    }

    private string? GetComment(Token[] tokens, ref Int32 index)
    {
        if (tokens.Length <= index)
            return null;

        if (tokens[index].Value == ":")
        {
            var line = tokens[index].ScriptLine.Value;
            return line.Substring(tokens[index].Range.ToChar + 1);
        }

        return null;
    }
}