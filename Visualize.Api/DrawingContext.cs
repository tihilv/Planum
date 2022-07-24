using System.Drawing;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;
using Point = Visualize.Api.Geometry.Point;

namespace Visualize.Api;

public class DrawingContext
{
    private readonly Point _zeroRealPoint;
    private readonly Point _zeroGraphicsPoint;
    private readonly double _zoom;

    private readonly Action<IEnumerable<IVectorPrimitive>, Graphics, DrawingContext> _drawingAction;

    public DrawingContext(Point zeroRealPoint, Point zeroGraphicsPoint, Double zoom, Action<IEnumerable<IVectorPrimitive>, Graphics, DrawingContext> drawingAction)
    {
        _zeroRealPoint = zeroRealPoint;
        _zeroGraphicsPoint = zeroGraphicsPoint;
        _zoom = zoom;
        _drawingAction = drawingAction;
    }

    public RectangleF ToGraphics(RectangleD rectangle)
    {
        var pt = ToGraphics(rectangle.TopLeft);
        return new RectangleF(pt.X, pt.Y, ToGraphics(rectangle.Width), ToGraphics(rectangle.Height));
    }
    
    public PointF ToGraphics(Point realPoint)
    {
        return realPoint.Subtract(_zeroRealPoint).Multiply(_zoom).Add(_zeroGraphicsPoint).ToPointF();
    }
    
    public float ToGraphics(double realDistance)
    {
        return (float)(realDistance*_zoom);
    }

    public void RasterizePrimitives(IEnumerable<IVectorPrimitive> primitives, Graphics g)
    {
        _drawingAction(primitives, g, this);
    }
}