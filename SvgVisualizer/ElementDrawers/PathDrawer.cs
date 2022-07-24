using Svg;
using Svg.Pathing;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class PathDrawer: ElementDrawerBase<SvgPath>
{
    protected override IVectorPrimitive DoProcess(SvgPath element)
    {
        var primitive = new PathPrimitive(((SvgColourServer)element.Color).Colour);
        Point start = Point.Empty;
        foreach (var segment in element.PathData)
        {
            if (segment is SvgMoveToSegment moveTo)
            {
                var pt = new Point(moveTo.End.X, moveTo.End.Y);
                if (moveTo.IsRelative)
                    start = start.Add(pt);
                else
                    start = pt;
            } 
            else if (segment is SvgLineSegment line)
            {
                var end = new Point(line.End.X, line.End.Y);
                if (line.IsRelative)
                    end = start.Add(end);

                primitive.Lines.Add(new(start, end));
                start = end;
            }
            else if (segment is SvgCubicCurveSegment cubic)
            {
                var pt2 = new Point(cubic.FirstControlPoint.X, cubic.FirstControlPoint.Y);
                var pt3 = new Point(cubic.SecondControlPoint.X, cubic.SecondControlPoint.Y);
                var pt4 = new Point(cubic.End.X, cubic.End.Y);
                if (cubic.IsRelative)
                {
                    pt2 = start.Add(pt2);
                    pt3 = start.Add(pt3);
                    pt4 = start.Add(pt4);
                }

                primitive.Beziers.Add(new PathBezier(new[] { start, pt2, pt3, pt4 }));
                start = pt4;
            }
            else
                throw new InvalidOperationException($"Unknown PathData type '{segment.GetType().FullName}'");
        }
        return primitive;
    }
}