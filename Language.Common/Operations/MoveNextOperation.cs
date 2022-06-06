using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class MoveNextOperation : IOperation
{
    public static readonly IOperation Instance = new MoveNextOperation();

    public OperationDefinition Definition => DefaultOperationDefinitions.MoveElementNext;
    
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.OfType<IGroupableSemantic>().Any();
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        foreach (var selectedElement in selectedElements)
        {
            var thisMovingSyntaxElement = selectedElement.SyntaxElements.FirstOrDefault();
            var commonSyntaxParent = thisMovingSyntaxElement?.Parent;
            if (commonSyntaxParent != null)
            {
                var neighbourGroupableElement = FindNeighbourGroupableElement(allElements, selectedElement, commonSyntaxParent);
                if (neighbourGroupableElement != null)
                {
                    SplitCurrentMovingSyntaxElement(allElements, commonSyntaxParent, thisMovingSyntaxElement!, selectedElement);
                    
                    MoveSyntaxElement(neighbourGroupableElement, commonSyntaxParent, thisMovingSyntaxElement!);
                }
            }
        }
    }

    private void SplitCurrentMovingSyntaxElement(ISemanticElement[] allElements, CompositeSyntaxElement commonSyntaxParent, SyntaxElement thisMovingSyntaxElement, ISemanticElement selectedElement)
    {
        foreach (var element in allElements)
        {
            if (element != selectedElement && element is IGroupableSemantic groupable)
            {
                if (element.SyntaxElements.Any(e => e.Equals(thisMovingSyntaxElement)))
                {
                    MoveSyntaxElement(groupable, commonSyntaxParent, thisMovingSyntaxElement);
                }
            }
        }
    }

    private void MoveSyntaxElement(IGroupableSemantic groupableElementToExtract, CompositeSyntaxElement commonSyntaxParent, SyntaxElement thisMovingSyntaxElement)
    {
        foreach (var syntaxElementToMove in groupableElementToExtract.GroupableSyntaxElements.Reverse())
        {
            if (syntaxElementToMove.Parent != null)
                commonSyntaxParent.Make(syntaxElementToMove).Deleted();

            commonSyntaxParent.Make(syntaxElementToMove).Before(thisMovingSyntaxElement);

        }
    }

    private IGroupableSemantic? FindNeighbourGroupableElement(ISemanticElement[] allElements, ISemanticElement selectedElement, CompositeSyntaxElement commonSyntaxParent)
    {
        var index = Array.IndexOf(allElements, selectedElement) + 1;
        while (index < allElements.Length && (!(allElements[index] is IGroupableSemantic) || allElements[index].SyntaxElements.FirstOrDefault()?.Parent != commonSyntaxParent))
            index++;

        if (index < allElements.Length)
            return (IGroupableSemantic)allElements[index];

        return null;
    }
}