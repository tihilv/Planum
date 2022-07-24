using System.Drawing;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace Visualize.Api;

public interface IVectorImage
{
    void Add(IVectorPrimitive primitive);
    void Clear();

    void Rasterize(RectangleD modelRectangle, RectangleD graphicsRectangle, Graphics g);

    RectangleD GetBoundaries();
}