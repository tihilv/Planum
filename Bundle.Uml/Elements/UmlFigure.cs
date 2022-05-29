using System.Text;

namespace Bundle.Uml.Elements;

public struct UmlFigure
{
    public readonly UmlFigureType Type;
    public readonly string Text;
    public readonly string? Stereotype;
    public readonly string? Alias;

    public UmlFigure(UmlFigureType type, String text, String? stereotype = null, String? alias = null)
    {
        Type = type;
        Text = text;
        Stereotype = stereotype;
        Alias = alias;
    }

    public override String ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"({Text}: {Type})");
        if (Alias != null)
            sb.Append($" as {Alias}");

        if (Stereotype != null)
            sb.Append($" << {Stereotype} >>");

        return sb.ToString();
    }

    public UmlFigure With(string? text = null, string? alias = null)
    {
        return new UmlFigure(Type, text ?? Text, Stereotype, alias??Alias);
    }
}