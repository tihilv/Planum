using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class UngroupOperation : IOperation
{
    public static readonly IOperation Instance = new UngroupOperation();

    private UngroupOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.Ungroup;
    
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.OfType<IGroupSemantic>().Any();
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        foreach (var selectedElement in selectedElements)
            if (selectedElement is IGroupSemantic group)
                Ungroup(group);
    }

    private void Ungroup(IGroupSemantic group)
    {
        var groupSyntaxElement = group.GroupSyntaxElement;
        var parentSyntaxElement = groupSyntaxElement.Parent!;
        
        var children = groupSyntaxElement.Children.ToArray();
        foreach (var child in children)
        {
            groupSyntaxElement.Make(child).Deleted();
            parentSyntaxElement.Make(child).Before(groupSyntaxElement);
        }

        parentSyntaxElement.Make(groupSyntaxElement).Deleted();
    }
}