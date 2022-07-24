using Svg;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class EllipseDrawer: ElementDrawerBase<SvgEllipse>
{
    protected override IVectorPrimitive DoProcess(SvgEllipse element)
    {
        return new EllipsePrimitive(new RectangleD(new Point(element.CenterX.Value, element.CenterY), 2.0 * element.RadiusX.Value, 2.0 * element.RadiusY.Value), ((SvgColourServer)element.Color).Colour, ((SvgColourServer)element.Fill).Colour);
    }
}