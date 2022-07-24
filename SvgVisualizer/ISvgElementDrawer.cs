using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer;

internal interface ISvgElementDrawer
{
    Type SvgType { get; }
    IVectorPrimitive Process(SvgElement element);
}