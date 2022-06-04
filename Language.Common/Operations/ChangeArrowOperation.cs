using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Common.Primitives;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class ChangeArrowOperation : IOperation
{
    public static readonly IOperation Instance = new ChangeArrowOperation();

    private ChangeArrowOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.ChangeArrow;
    
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.OfType<IArrowedSemantic>().Any();
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        var newText = (Arrow)arguments[0];

        foreach (var selectedElement in selectedElements)
            if (selectedElement is IArrowedSemantic arrowedElement)
                arrowedElement.Arrow = newText;
    }
}