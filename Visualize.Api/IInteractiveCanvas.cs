using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api;

public interface IInteractiveCanvas
{
    void Move(PointF graphicsVector);
    void Resize(RectangleF graphicsRectangle);
    void Refresh(Graphics g);
    void Zoom(PointF graphicsPoint, float zoomFactor);
    void ZoomToExtents();
    PointD ToModel(PointF graphicsPoint);
}