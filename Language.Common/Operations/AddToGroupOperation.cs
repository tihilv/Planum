using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class AddToGroupOperation : IOperation
{
    public static readonly IOperation Instance = new AddToGroupOperation();

    private AddToGroupOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.AddToGroup;
    
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        int count = 0;
        bool isFirstGroup = false;
        foreach (var element in elements)
        {
            if (count == 0 && element is IGroupSemantic)
                isFirstGroup = true;

            if (element is IGroupableSemantic)
                count++;
        }
        return count > 0 && isFirstGroup;
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        var first = true;
        IGroupSemantic? group = null;
        foreach (var el in selectedElements)
        {
            if (first)
            {
                first = false;
                group = (IGroupSemantic)el;
            }
            else
            {
                if (el is IGroupableSemantic groupable)
                    AddToGroup(group, groupable);
            }
        }
    }

    private void AddToGroup(IGroupSemantic group, IGroupableSemantic element)
    {
        foreach (var syntaxElement in element.GroupableSyntaxElements)
        {
            if (syntaxElement.Parent != null)
                syntaxElement.Parent.Make(syntaxElement).Deleted();
            
            group.GroupSyntaxElement.Make(syntaxElement).Last();
        }
    }
}