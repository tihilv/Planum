using System.Drawing;
using Visualize.Api.Geometry;
using Visualize.Api.Primitives;

namespace Visualize.Api;

public interface IVectorImage
{
    long Generation { get; }
    IEnumerable<IVectorPrimitive> Primitives { get; }

    void Add(IVectorPrimitive primitive);
    void Clear();

    void Rasterize(RectangleD modelRectangle, RectangleF graphicsRectangle, Graphics g);

    RectangleD GetBoundaries();
}