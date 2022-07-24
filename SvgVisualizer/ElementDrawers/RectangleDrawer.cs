using Svg;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class RectangleDrawer: ElementDrawerBase<SvgRectangle>
{
    protected override IVectorPrimitive DoProcess(SvgRectangle element)
    {
        return new RectanglePrimitive(new RectangleD(element.X.Value, element.Y.Value, element.Width.Value, element.Height.Value), ((SvgColourServer)element.Color).Colour);
    }
}