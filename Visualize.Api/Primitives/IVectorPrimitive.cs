using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api.Primitives;

public interface IVectorPrimitive
{
    public Color ForeColor { get; }
    public Color BackColor { get; }
    RectangleD GetBoundaries();
}