using System.Drawing;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace Visualize.Api;

public class DrawingContext
{
    private readonly PointD _zeroRealPoint;
    private readonly PointF _zeroGraphicsPoint;
    private readonly double _zoom;

    private readonly Action<IEnumerable<IVectorPrimitive>, Graphics, DrawingContext>? _drawingAction;

    public DrawingContext(RectangleD modelRectangle, RectangleF graphicsRectangle, Action<IEnumerable<IVectorPrimitive>, Graphics, DrawingContext>? drawingAction)
    {
        _zeroRealPoint = modelRectangle.TopLeft;
        _zeroGraphicsPoint = graphicsRectangle.Location;
        _zoom = Math.Min((graphicsRectangle.Width-1) / modelRectangle.Width, (graphicsRectangle.Height-1) / modelRectangle.Height);
        _drawingAction = drawingAction;
    }

    public RectangleF ToGraphics(RectangleD rectangle)
    {
        var pt = ToGraphics(rectangle.TopLeft);
        return new RectangleF(pt.X, pt.Y, ToGraphics(rectangle.Width), ToGraphics(rectangle.Height));
    }
    
    public PointF ToGraphics(PointD realPoint)
    {
        return realPoint.Subtract(_zeroRealPoint).Multiply(_zoom).Add(_zeroGraphicsPoint);
    }

    public PointD ToModel(PointF graphicsPoint)
    {
        return new PointD((graphicsPoint.X - _zeroGraphicsPoint.X) / _zoom, (graphicsPoint.Y - _zeroGraphicsPoint.Y) / _zoom).Add(_zeroRealPoint);
    }

    public float ToGraphics(double realDistance)
    {
        return (float)(realDistance*_zoom);
    }

    public float ToModel(double modelDistance)
    {
        return (float)(modelDistance/_zoom);
    }

    
    public void RasterizePrimitives(IEnumerable<IVectorPrimitive> primitives, Graphics g)
    {
        _drawingAction?.Invoke(primitives, g, this);
    }
}