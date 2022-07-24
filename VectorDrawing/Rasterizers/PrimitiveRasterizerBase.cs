using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal abstract class PrimitiveRasterizerBase<T>: IPrimitiveRasterizer where T : class, IVectorPrimitive
{
    public Type PrimitiveType => typeof(T);

    public void Process(IVectorPrimitive primitive, Graphics g, DrawingContext context)
    {
        if (primitive is T typedElement)
            DoProcess(typedElement, g, context);
        else
            throw new ArgumentException($"Unable svg element type to process: {primitive.GetType().FullName}.");
    }
    
    protected Pen GetPen(IVectorPrimitive primitive)
    {
        return new Pen(primitive.ForeColor);
    }
    
    protected Brush GetFillBrush(IVectorPrimitive primitive)
    {
        return new SolidBrush(primitive.BackColor);
    }

    protected Brush GetBrush(IVectorPrimitive primitive)
    {
        return new SolidBrush(primitive.ForeColor);
    }

    protected abstract void DoProcess(T primitive, Graphics g, DrawingContext context);
}