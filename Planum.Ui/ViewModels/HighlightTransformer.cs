using System.Drawing;
using System.Drawing.Drawing2D;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace Planum.Ui.ViewModels;

public class HighlightTransformer : IColorTransformer
{
    private readonly Color _color;

    public HighlightTransformer(Color color)
    {
        _color = color;
    }

    public Pen GetPen(IVectorPrimitive primitive)
    {
        return new Pen(_color);
    }

    public Brush GetFillBrush(IVectorPrimitive primitive)
    {
        return new HatchBrush(HatchStyle.Percent10, _color, primitive.BackColor);
    }

    public Brush GetBrush(IVectorPrimitive primitive)
    {
        return new HatchBrush(HatchStyle.Percent50, _color, primitive.ForeColor);
    }
}