using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api.Primitives;

public class TextPrimitive: IVectorPrimitive
{
    private readonly RectangleD _rectangle;
    private readonly string _fontFamily;
    private readonly FontStyle _fontStyle;
    private readonly double _fontSize;
    private readonly string _text;
    private readonly Color _foreColor;

    public String FontFamily => _fontFamily;
    public RectangleD Rectangle => _rectangle;
    public FontStyle FontStyle => _fontStyle;
    public double FontSize => _fontSize;
    public String Text => _text;

    public TextPrimitive(RectangleD rectangle, String fontFamily, FontStyle fontStyle, Double fontSize, String text, Color foreColor)
    {
        _rectangle = rectangle;
        _fontFamily = fontFamily;
        _fontStyle = fontStyle;
        _fontSize = fontSize;
        _text = text;
        _foreColor = foreColor;
    }

    public Color ForeColor => _foreColor;
    public Color BackColor { get; }

    public RectangleD GetBoundaries() => _rectangle;
}