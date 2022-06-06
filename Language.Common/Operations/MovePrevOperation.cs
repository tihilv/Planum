using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class MovePrevOperation : IOperation
{
    public static readonly IOperation Instance = new MovePrevOperation();

    public OperationDefinition Definition => DefaultOperationDefinitions.MoveElementPrev;
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return MoveNextOperation.Instance.CanExecute(elements);
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        foreach (var selectedElement in selectedElements)
        {
            var commonSyntaxParent = selectedElement.SyntaxElements.FirstOrDefault()?.Parent;

            if (commonSyntaxParent != null)
            {
                var neighbour = FindNeighbourGroupableElement(allElements, selectedElement, commonSyntaxParent);
                if (neighbour != null)
                    MoveNextOperation.Instance.Execute(allElements, new[] { neighbour }, arguments);
            }

        }
    }
    
    private ISemanticElement? FindNeighbourGroupableElement(ISemanticElement[] allElements, ISemanticElement selectedElement, CompositeSyntaxElement commonSyntaxParent)
    {
        var index = Array.IndexOf(allElements, selectedElement) - 1;
        while (index >=0 && (!(allElements[index] is IGroupableSemantic) || allElements[index].SyntaxElements.FirstOrDefault()?.Parent != commonSyntaxParent))
            index--;

        if (index >=0)
            return allElements[index];

        return null;
    }
}