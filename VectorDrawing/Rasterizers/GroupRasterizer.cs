using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class GroupRasterizer : PrimitiveRasterizerBase<GroupPrimitive>
{
    protected override void DoProcess(GroupPrimitive primitive, Graphics g, DrawingContext context)
    {
        context.RasterizePrimitives(primitive.Children, g);
    }
}