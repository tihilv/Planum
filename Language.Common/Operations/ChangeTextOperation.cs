using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class ChangeTextOperation : IPropertyOperation
{
    public static readonly IPropertyOperation Instance = new ChangeTextOperation();

    private ChangeTextOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.ChangeText;

    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.OfType<ITextedSemantic>().Any();
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        var newText = (String)arguments[0];

        foreach (var selectedElement in selectedElements)
            if (selectedElement is ITextedSemantic textedElement)
                textedElement.Text = newText;
    }

    public Type PropertyType => typeof(string);
    public Object GetValue(ISemanticElement element)
    {
        return ((ITextedSemantic)element).Text;
    }
}