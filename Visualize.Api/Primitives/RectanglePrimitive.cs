using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api.Primitives;

public class RectanglePrimitive: IVectorPrimitive
{
    private readonly RectangleD _rectangle;
    private readonly Color _foreColor;

    public RectangleD Rectangle => _rectangle;

    public RectanglePrimitive(RectangleD rectangle, Color foreColor)
    {
        _rectangle = rectangle;
        _foreColor = foreColor;
    }

    public Color ForeColor => _foreColor;
    public Color BackColor { get; }

    public RectangleD GetBoundaries() => _rectangle;
}