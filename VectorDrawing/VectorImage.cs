using System.Drawing;
using VectorDrawing.Rasterizers;
using Visualize.Api;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace VectorDrawing;

public class VectorImage : IVectorImage
{
    private readonly List<IVectorPrimitive> _primitives;
    
    private static readonly Dictionary<Type, IPrimitiveRasterizer> _primitiveRasterizers;

    static VectorImage()
    {
        _primitiveRasterizers = GetPrimitiveRasterizers().ToDictionary(d => d.PrimitiveType);
    }

    public VectorImage()
    {
        _primitives = new List<IVectorPrimitive>();
    }

    private static IEnumerable<IPrimitiveRasterizer> GetPrimitiveRasterizers()
    {
        yield return new DocumentRasterizer();
        yield return new DefinitionListRasterizer();
        yield return new GroupRasterizer();
        yield return new LinkRasterizer();
        yield return new RectangleRasterizer();
        yield return new TextRasterizer();
        yield return new EllipseRasterizer();
        yield return new PathRasterizer();
        yield return new PolygonRasterizer();
    }
    
    public void Clear()
    {
        _primitives.Clear();
    }

    public void Rasterize(RectangleD modelRectangle, RectangleD graphicsRectangle, Graphics g)
    {
        var zoom = Math.Min(graphicsRectangle.Width / modelRectangle.Width, graphicsRectangle.Height / modelRectangle.Height);
        var context = new DrawingContext(modelRectangle.TopLeft, graphicsRectangle.TopLeft, zoom, RasterizePrimitives);

        RasterizePrimitives(_primitives, g, context);
    }

    private void RasterizePrimitives(IEnumerable<IVectorPrimitive> primitives, Graphics g, DrawingContext context)
    {
        foreach (var primitive in primitives)
            _primitiveRasterizers[primitive.GetType()].Process(primitive, g, context);
    }
    
    public RectangleD GetBoundaries()
    {
        var result = _primitives[0].GetBoundaries();
        foreach (var primitive in _primitives.Skip(1))
            result = result.Union(primitive.GetBoundaries());

        return result;
    }

    public void Add(IVectorPrimitive primitive)
    {
        _primitives.Add(primitive);
    }
}