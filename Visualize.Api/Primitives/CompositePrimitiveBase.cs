using System.Drawing;
using Visualize.Api.Geometry;

namespace Visualize.Api.Primitives;

public abstract class CompositePrimitiveBase : IVectorPrimitive
{
    private readonly List<IVectorPrimitive> _children;

    protected CompositePrimitiveBase()
    {
        _children = new List<IVectorPrimitive>();
    }

    public Color ForeColor => Color.Transparent;
    public Color BackColor => Color.Transparent;

    public List<IVectorPrimitive> Children => _children;

    public RectangleD GetBoundaries()
    {
        if (!_children.Any())
            return RectangleD.Empty;

        var result = _children[0].GetBoundaries();
        foreach (var primitive in _children.Skip(1))
            result = result.Union(primitive.GetBoundaries());

        return result;
    }
}