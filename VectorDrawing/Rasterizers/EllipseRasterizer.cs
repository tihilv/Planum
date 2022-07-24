using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class EllipseRasterizer: PrimitiveRasterizerBase<EllipsePrimitive>
{
    protected override void DoProcess(EllipsePrimitive primitive, Graphics g, DrawingContext context)
    {
        using (var pen = GetPen(primitive))
        {
            var topLeft = context.ToGraphics(primitive.Rectangle.TopLeft);
            if (primitive.BackColor != Color.Transparent)
                using (var brush = GetFillBrush(primitive))
                    g.FillEllipse(brush, topLeft.X, topLeft.Y, context.ToGraphics(primitive.Rectangle.Width), context.ToGraphics(primitive.Rectangle.Height));

            g.DrawEllipse(pen, topLeft.X, topLeft.Y, context.ToGraphics(primitive.Rectangle.Width), context.ToGraphics(primitive.Rectangle.Height));
        }
    }
}