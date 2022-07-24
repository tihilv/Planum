using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal interface IPrimitiveRasterizer
{
    Type PrimitiveType { get; }

    void Process(IVectorPrimitive primitive, Graphics g, DrawingContext context);
}