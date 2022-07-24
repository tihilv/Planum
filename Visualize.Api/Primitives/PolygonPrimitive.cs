using System.Drawing;
using Visualize.Api.Geometry;
using Point = Visualize.Api.Geometry.Point;

namespace Visualize.Api.Primitives;

public class PolygonPrimitive: IVectorPrimitive
{
    private readonly List<Point> _points;
    private readonly Color _foreColor;
    private readonly Color _backColor;

    public PolygonPrimitive(List<Point> points, Color foreColor, Color backColor)
    {
        _points = points;
        _foreColor = foreColor;
        _backColor = backColor;
    }

    public List<Point> Points => _points;

    public Color ForeColor => _foreColor;
    public Color BackColor => _backColor;

    public RectangleD GetBoundaries()
    {
        if (!_points.Any())
            return RectangleD.Empty;
        
        Point minPoint = _points[0];
        Point maxPoint = _points[0];

        foreach (var point in _points.Skip(1))
        {
            minPoint = minPoint.MinCombined(point);
            maxPoint = maxPoint.MaxCombined(point);
        }

        return new RectangleD(minPoint.X, minPoint.Y, maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y);
    }
}