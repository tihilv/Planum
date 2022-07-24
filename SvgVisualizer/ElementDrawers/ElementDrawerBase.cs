using Svg;
using Visualize.Api.Primitives;

namespace SvgVisualizer.ElementDrawers;

internal abstract class ElementDrawerBase<T>: ISvgElementDrawer where T : SvgElement
{
    public Type SvgType => typeof(T);

    public IVectorPrimitive Process(SvgElement element)
    {
        if (element is T typedElement)
            return DoProcess(typedElement);
        
        throw new ArgumentException($"Unable svg element type to process: {element.GetType().FullName}.");
    }

    protected abstract IVectorPrimitive DoProcess(T element);
}