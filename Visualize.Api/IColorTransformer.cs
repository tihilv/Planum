using System.Drawing;
using Visualize.Api.Primitives;

namespace Visualize.Api;

public interface IColorTransformer
{
    Pen GetPen(IVectorPrimitive primitive);
    Brush GetFillBrush(IVectorPrimitive primitive);
    Brush GetBrush(IVectorPrimitive primitive);
}