using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class LinkRasterizer : PrimitiveRasterizerBase<LinkPrimitive>
{
    public LinkRasterizer(IColorTransformer colorTransformer) : base(colorTransformer)
    {
    }

    protected override void DoProcess(LinkPrimitive primitive, Graphics g, DrawingContext context)
    {
        context.RasterizePrimitives(primitive.Children, g);
    }
}