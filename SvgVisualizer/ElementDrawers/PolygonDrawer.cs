using Svg;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class PolygonDrawer: ElementDrawerBase<SvgPolygon>
{
    protected override IVectorPrimitive DoProcess(SvgPolygon element)
    {
        List<PointD> points = new List<PointD>();
        for (var index = 0; index < element.Points.Count; index+=2)
        {
            var x = element.Points[index].Value;
            var y = element.Points[index+1].Value;
            points.Add(new PointD(x, y));
        }

        return new PolygonPrimitive(points, ((SvgColourServer)element.Color).Colour, ((SvgColourServer)element.Fill).Colour);
    }
}