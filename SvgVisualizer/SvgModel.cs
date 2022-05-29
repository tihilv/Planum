using System.Drawing;
using Svg;
using Svg.Transforms;

namespace SvgVisualizer;

public class SvgModel
{
    private SvgDocument? _document;

    public void UpdateModel(string svg)
    {
        _document = SvgDocument.FromSvg<SvgDocument>(svg);
    }

    public Bitmap? CreateBitmap()
    {
        _document.Transforms = new SvgTransformCollection();
        _document.AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin);
        _document.Width = 500;
        _document.Height = 500;
        return _document?.Draw();
    }
}