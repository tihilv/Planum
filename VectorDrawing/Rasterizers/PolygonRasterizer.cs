using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class PolygonRasterizer: PrimitiveRasterizerBase<PolygonPrimitive>
{
    protected override void DoProcess(PolygonPrimitive primitive, Graphics g, DrawingContext context)
    {
        using (var pen = GetPen(primitive))
        {
            var points = primitive.Points.Select(context.ToGraphics).ToArray();

            if (primitive.BackColor != Color.Transparent)
                using  (var brush = GetFillBrush(primitive))
                    g.FillPolygon(brush, points);
            
            g.DrawPolygon(pen, points);
        }
    }
}