using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class RectangleRasterizer: PrimitiveRasterizerBase<RectanglePrimitive>
{
    public RectangleRasterizer(IColorTransformer colorTransformer) : base(colorTransformer)
    {
    }

    protected override void DoProcess(RectanglePrimitive primitive, Graphics g, DrawingContext context)
    {
        using (var pen = GetPen(primitive))
        {
            var topLeft = context.ToGraphics(primitive.Rectangle.TopLeft);
            g.DrawRectangle(pen, topLeft.X, topLeft.Y, context.ToGraphics(primitive.Rectangle.Width), context.ToGraphics(primitive.Rectangle.Height));
        }
    }
}