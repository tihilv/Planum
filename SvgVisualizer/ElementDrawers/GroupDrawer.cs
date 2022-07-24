using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class GroupDrawer : ElementDrawerBase<SvgGroup>
{
    protected override IVectorPrimitive DoProcess(SvgGroup element)
    {
        return new GroupPrimitive();
    }
}