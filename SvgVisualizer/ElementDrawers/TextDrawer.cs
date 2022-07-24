using System.Drawing;
using Svg;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal class TextDrawer: ElementDrawerBase<SvgText>
{
    protected override IVectorPrimitive DoProcess(SvgText element)
    {
        var fontStyle = FontStyle.Regular;
        if (element.FontWeight == SvgFontWeight.Bold)
            fontStyle = fontStyle | FontStyle.Bold;

        var rect = new RectangleD(element.X.Single().Value, element.Y.Single().Value-element.FontSize.Value, element.TextLength.Value, element.FontSize.Value);
        
        return new TextPrimitive(rect, element.FontFamily, fontStyle, element.FontSize.Value, element.Text, ((SvgColourServer)element.Color).Colour);
    }
}