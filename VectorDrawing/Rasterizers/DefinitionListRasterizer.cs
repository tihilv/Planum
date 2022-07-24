using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class DefinitionListRasterizer : PrimitiveRasterizerBase<DefinitionListPrimitive>
{
    protected override void DoProcess(DefinitionListPrimitive primitive, Graphics g, DrawingContext context)
    {
        context.RasterizePrimitives(primitive.Children, g);
    }
}