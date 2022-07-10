using System.Text;
using Bundle.Uml.Elements;
using Language.Api;
using Language.Api.Syntax;
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
        string? url = null;
        if (arrow != null)
        {
            secondFigure = GetFigure(tokens, ref index);
            if (secondFigure != null && !string.IsNullOrEmpty(secondFigure.Value.Url))
            {
                url = secondFigure.Value.Url;
                secondFigure = secondFigure.Value.With(url: String.Empty);
            }
        }
        
        var comment = GetComment(tokens, ref index);

        return new ParseResult(new UmlSyntaxElement(firstFigure.Value, arrow, secondFigure, comment, url));
    }

    public SynthesizeResult? Synthesize(SyntaxElement element)
    {
        if (element is UmlSyntaxElement el)
        {
            StringBuilder sb = new StringBuilder();
            SynthesizeFigure(el.FirstFigure, sb);

            if (el.Arrow != null)
            {
                sb.Append(" ");
                sb.Append(ArrowParser.Instance.Synthesize(el.Arrow.Value));
            }

            if (el.SecondFigure != null)
            {
                sb.Append(" ");
                SynthesizeFigure(el.SecondFigure.Value, sb);
            }
        

            if (!string.IsNullOrEmpty(el.Comment))
            {
                sb.Append(" : ");
                sb.Append(el.Comment);
            }

            return new SynthesizeResult(sb.ToString());
        }

        return null;
    }

    private void SynthesizeFigure(UmlFigure figure, StringBuilder sb)
    {
        sb.Append(figure.Type.ToString().ToLower());
        sb.Append(" \"");
        sb.Append(figure.Text);
        sb.Append("\"");
        if (!string.IsNullOrEmpty(figure.Alias))
        {
            sb.Append(" as \"");
            sb.Append(figure.Alias);
            sb.Append("\"");
        }

        if (!string.IsNullOrEmpty(figure.Stereotype))
        {
            sb.Append(" <<");
            sb.Append(figure.Stereotype);
            sb.Append(">>");
        }

        if (!string.IsNullOrEmpty(figure.Url))
        {
            sb.Append(" [[");
            sb.Append(figure.Url);
            sb.Append("]]");
        }
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
        if (tokens.Length > index && tokens[index].Value.StartsWith("<<") && tokens[index].Value.EndsWith(">>"))
        {
            stereotype = tokens[index].Value.Substring(2, tokens[index].Value.Length - 4).Trim();
            index++;
        }

        string? url = null;
        if (tokens.Length > index && tokens[index].Value.StartsWith("[[") && tokens[index].Value.EndsWith("]]"))
        {
            url = tokens[index].Value.Substring(2, tokens[index].Value.Length - 4).Trim();
            index++;
        }
        
        figureName = figureName.Trim('"');
        return new UmlFigure(figureType, figureName, stereotype, alias, url);
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