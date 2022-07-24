using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class LinkDrawer : ElementDrawerBase<SvgAnchor>
{
    protected override IVectorPrimitive DoProcess(SvgAnchor element)
    {
        return new LinkPrimitive(element.Href);
    }
}