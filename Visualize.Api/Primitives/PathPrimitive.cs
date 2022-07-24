using System.Drawing;
using Visualize.Api.Geometry;
using Point = Visualize.Api.Geometry.Point;

namespace Visualize.Api.Primitives;

public class PathPrimitive: IVectorPrimitive
{
    private readonly Color _foreColor;

    private readonly List<PathLine> _lines;
    private readonly List<PathBezier> _beziers;

    public PathPrimitive(Color foreColor)
    {
        _foreColor = foreColor;
        
        _lines = new List<PathLine>();
        _beziers = new List<PathBezier>();
    }

    public List<PathLine> Lines => _lines;
    public List<PathBezier> Beziers => _beziers;

    public Color ForeColor => _foreColor;
    public Color BackColor { get; }

    public RectangleD GetBoundaries()
    {
        if (!_lines.Any())
            return RectangleD.Empty;
        
        Point minPoint = _lines[0].Start.MinCombined(_lines[0].End);
        Point maxPoint = _lines[0].Start.MaxCombined(_lines[0].End);

        foreach (var line in _lines.Skip(1))
        {
            minPoint = minPoint.MinCombined(line.Start).MinCombined(line.End);
            maxPoint = maxPoint.MaxCombined(line.Start).MaxCombined(line.End);
        }

        return new RectangleD(minPoint.X, minPoint.Y, maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y);
    }
}

public readonly struct PathLine
{
    public readonly Point Start;
    public readonly Point End;

    public PathLine(Point start, Point end)
    {
        Start = start;
        End = end;
    }

    public override String ToString()
    {
        return $"{Start} - {End}";
    }
}

public readonly struct PathBezier
{
    public readonly Point[] Points;

    public PathBezier(Point[] points)
    {
        Points = points;
    }
}