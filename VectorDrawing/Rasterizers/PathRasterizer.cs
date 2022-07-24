using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class PathRasterizer: PrimitiveRasterizerBase<PathPrimitive>
{
    protected override void DoProcess(PathPrimitive primitive, Graphics g, DrawingContext context)
    {
        using (var pen = GetPen(primitive))
        {
            foreach (var line in primitive.Lines)
            {
                var start = context.ToGraphics(line.Start);
                var end = context.ToGraphics(line.End);
                g.DrawLine(pen, start, end);
            }
            
            foreach (var line in primitive.Beziers)
            {
                var points = line.Points.Select(context.ToGraphics).ToArray();
                g.DrawBeziers(pen, points);
            }

        }
    }
}