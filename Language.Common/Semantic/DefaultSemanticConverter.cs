using Language.Api;
using Language.Api.Semantic;
using Language.Api.Syntax;

namespace Language.Common.Semantic;

public class DefaultSemanticConverter : ISemanticConverter
{
    private readonly IBundleBuilder _bundleBuilder;

    public DefaultSemanticConverter(IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
    }

    public IEnumerable<ISemanticElement> GetSemanticElements(RootSyntaxElement rootSyntaxElement)
    {
        var context = new SyntaxToSemanticContext();
        var map = new Dictionary<ISemanticElement, ISyntaxToSemanticTransfer>();
        var result = VisitComposite(rootSyntaxElement, context, map);
        foreach (var element in result)
        {
            if (map[element] is IFinishableSyntaxToSemanticTransfer finishable && !finishable.Postprocess(element, context))
                    continue;

            yield return element;
        }
    }

    private IEnumerable<ISemanticElement> VisitComposite(CompositeSyntaxElement composite, SyntaxToSemanticContext context, Dictionary<ISemanticElement, ISyntaxToSemanticTransfer> map)
    {
        foreach (var child in composite.Children)
        {
            foreach (var transfer in _bundleBuilder.GetSyntaxToSemanticTransfers())
            {
                var semanticElements = transfer.TryConvert(child, context);
                foreach (var el in semanticElements)
                {
                    map.Add(el, transfer);
                    yield return el;
                }
            }

            if (child is CompositeSyntaxElement childComposite)
                foreach (var subChild in VisitComposite(childComposite, context, map))
                    yield return subChild;
        }
    }

    private class SemanticToTransferMap
    {
        public readonly Dictionary<ISemanticElement, ISyntaxToSemanticTransfer> Map = new Dictionary<ISemanticElement, ISyntaxToSemanticTransfer>();
    }
}