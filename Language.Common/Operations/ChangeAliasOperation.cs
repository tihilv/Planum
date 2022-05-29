using Language.Api.Operations;
using Language.Api.Semantic;

namespace Language.Common.Operations;

public class ChangeAliasOperation : IOperation
{
    public static readonly IOperation Instance = new ChangeAliasOperation();

    private ChangeAliasOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.ChangeAlias;

    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.OfType<IAliased>().Any();
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        var newAlias = (String)arguments[0];

        foreach (var selectedElement in selectedElements)
            if (selectedElement is IAliased textedElement)
                textedElement.Alias = newAlias;
    }
}