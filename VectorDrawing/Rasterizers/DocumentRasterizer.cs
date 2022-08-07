using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class DocumentRasterizer : PrimitiveRasterizerBase<DocumentPrimitive>
{
    public DocumentRasterizer(IColorTransformer colorTransformer) : base(colorTransformer)
    {
    }

    protected override void DoProcess(DocumentPrimitive primitive, Graphics g, DrawingContext context)
    {
        context.RasterizePrimitives(primitive.Children, g);
    }
}