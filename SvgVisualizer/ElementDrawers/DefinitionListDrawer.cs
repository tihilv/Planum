using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class DefinitionListDrawer : ElementDrawerBase<SvgDefinitionList>
{
    protected override IVectorPrimitive DoProcess(SvgDefinitionList element)
    {
        return new DefinitionListPrimitive();
    }
}