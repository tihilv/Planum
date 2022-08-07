using System.Drawing;
using VectorDrawing.Rasterizers;
using Visualize.Api;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace VectorDrawing;

public class VectorImage : IVectorImage
{
    private readonly Dictionary<Type, IPrimitiveRasterizer> _primitiveRasterizers;

    private readonly IColorTransformer _colorTransformer;
    
    private readonly List<IVectorPrimitive> _primitives;

    public Int64 Generation { get; private set; }
    public IEnumerable<IVectorPrimitive> Primitives => _primitives;

    public VectorImage(IColorTransformer? colorTransformer = null)
    {
        _colorTransformer = colorTransformer ?? new DefaultColorTransformer();
        
        _primitiveRasterizers = GetPrimitiveRasterizers().ToDictionary(d => d.PrimitiveType);
        _primitives = new List<IVectorPrimitive>();
    }

    private IEnumerable<IPrimitiveRasterizer> GetPrimitiveRasterizers()
    {
        yield return new DocumentRasterizer(_colorTransformer);
        yield return new DefinitionListRasterizer(_colorTransformer);
        yield return new GroupRasterizer(_colorTransformer);
        yield return new LinkRasterizer(_colorTransformer);
        yield return new RectangleRasterizer(_colorTransformer);
        yield return new TextRasterizer(_colorTransformer);
        yield return new EllipseRasterizer(_colorTransformer);
        yield return new PathRasterizer(_colorTransformer);
        yield return new PolygonRasterizer(_colorTransformer);
    }
    
    public void Clear()
    {
        if (_primitives.Any())
        {
            _primitives.Clear();
            Generation++;
        }
    }

    public void Rasterize(RectangleD modelRectangle, RectangleF graphicsRectangle, Graphics g)
    {
        var context = new DrawingContext(modelRectangle, graphicsRectangle, RasterizePrimitives);

        RasterizePrimitives(_primitives, g, context);
    }

    private void RasterizePrimitives(IEnumerable<IVectorPrimitive> primitives, Graphics g, DrawingContext context)
    {
        foreach (var primitive in primitives)
            _primitiveRasterizers[primitive.GetType()].Process(primitive, g, context);
    }
    
    public RectangleD GetBoundaries()
    {
        if (!_primitives.Any())
            return RectangleD.Empty;

        var result = _primitives[0].GetBoundaries();
        foreach (var primitive in _primitives.Skip(1))
            result = result.Union(primitive.GetBoundaries());

        return result;
    }

    public void Add(IVectorPrimitive primitive)
    {
        _primitives.Add(primitive);
        Generation++;
    }
    
    private class DefaultColorTransformer : IColorTransformer
    {
        public Pen GetPen(IVectorPrimitive primitive)
        {
            return new Pen(primitive.ForeColor);
        }
    
        public Brush GetFillBrush(IVectorPrimitive primitive)
        {
            return new SolidBrush(primitive.BackColor);
        }

        public Brush GetBrush(IVectorPrimitive primitive)
        {
            return new SolidBrush(primitive.ForeColor);
        }

    }
}