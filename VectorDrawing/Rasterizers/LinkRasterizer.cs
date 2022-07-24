using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class LinkRasterizer : PrimitiveRasterizerBase<LinkPrimitive>
{
    protected override void DoProcess(LinkPrimitive primitive, Graphics g, DrawingContext context)
    {
        context.RasterizePrimitives(primitive.Children, g);
    }
}