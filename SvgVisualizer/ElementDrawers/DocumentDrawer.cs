using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class DocumentDrawer : ElementDrawerBase<SvgDocument>
{
    protected override IVectorPrimitive DoProcess(SvgDocument element)
    {
        return new DocumentPrimitive();
    }
}