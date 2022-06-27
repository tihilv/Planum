using System.Text;

namespace Bundle.Uml.Elements;

public struct UmlFigure
{
    public readonly UmlFigureType Type;
    public readonly string Text;
    public readonly string? Stereotype;
    public readonly string? Alias;
    public readonly string? Url;

    public UmlFigure(UmlFigureType type, String text, String? stereotype = null, String? alias = null, String? url = null)
    {
        Type = type;
        Text = text;
        Stereotype = stereotype;
        Alias = alias;
        Url = url;
    }

    public override String ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"({Text}: {Type})");
        if (Alias != null)
            sb.Append($" as {Alias}");

        if (Stereotype != null)
            sb.Append($" << {Stereotype} >>");

        if (Url != null)
            sb.Append($" [[ {Url} ]]");

        return sb.ToString();
    }

    public UmlFigure With(string? text = null, string? alias = null, string? url = null)
    {
        return new UmlFigure(Type, text ?? Text, Stereotype, alias ?? Alias, (url == string.Empty) ? null : (url ?? Url));
    }

    public bool IsSingleFigure => string.IsNullOrEmpty(Alias) && string.IsNullOrEmpty(Stereotype) && string.IsNullOrEmpty(Url);
}