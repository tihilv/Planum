using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api.Primitives;

public class EllipsePrimitive: IVectorPrimitive
{
    private readonly RectangleD _rectangle;
    private readonly Color _foreColor;
    private readonly Color _backColor;

    public RectangleD Rectangle => _rectangle;

    public EllipsePrimitive(RectangleD rectangle, Color foreColor, Color backColor)
    {
        _rectangle = rectangle;
        _foreColor = foreColor;
        _backColor = backColor;
    }

    public Color ForeColor => _foreColor;
    public Color BackColor => _backColor;
    
    public RectangleD GetBoundaries() => _rectangle;
}