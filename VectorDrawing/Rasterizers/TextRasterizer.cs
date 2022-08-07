using System.Drawing;
using Visualize.Api;
using Visualize.Api.Primitives;

namespace VectorDrawing.Rasterizers;

internal class TextRasterizer: PrimitiveRasterizerBase<TextPrimitive>
{
    public TextRasterizer(IColorTransformer colorTransformer) : base(colorTransformer)
    {
    }

    protected override void DoProcess(TextPrimitive primitive, Graphics g, DrawingContext context)
    {
        using (var brush = GetBrush(primitive))
        using (var fontFamily = GetFontFamily(primitive.FontFamily))
        using (var font = AdjustFontToPixelSize(fontFamily, context.ToGraphics(primitive.FontSize), primitive.FontStyle))
        {
            
            var rect = context.ToGraphics(primitive.Rectangle);
            var measure = g.MeasureString(primitive.Text, font);
            g.DrawString(primitive.Text, font, brush, rect);
        }
    }

    private static Font AdjustFontToPixelSize(FontFamily fontFamily, float requiredSizeInPx, FontStyle fontStyle)
    {
        var tempFont = new Font(fontFamily, requiredSizeInPx, fontStyle);
        var ratio = requiredSizeInPx / tempFont.GetHeight();
        tempFont.Dispose();
        return new Font(fontFamily, requiredSizeInPx*ratio, fontStyle);
    }

    private static FontFamily GetFontFamily(string family)
    {
        if (family == "sans-serif")
            return FontFamily.GenericSansSerif;

        return new FontFamily(family);
    }
}