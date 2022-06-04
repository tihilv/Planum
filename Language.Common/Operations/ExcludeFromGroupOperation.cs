using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public class ExcludeFromGroupOperation : IOperation
{
    public static readonly IOperation Instance = new ExcludeFromGroupOperation();

    private ExcludeFromGroupOperation()
    {
    }

    public OperationDefinition Definition => DefaultOperationDefinitions.ExcludeFromGroup;
    
    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.Any(e => e is IGroupableSemantic && e.SyntaxElements.FirstOrDefault()?.Parent is IGroupSyntaxElement);
    }

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        foreach (var element in selectedElements)
        {
            if (element is IGroupableSemantic groupable)
            {
                var syntaxElementToProcess = groupable.GroupableSyntaxElements.FirstOrDefault();
                if (syntaxElementToProcess?.Parent is IGroupSyntaxElement)
                {
                    RemoveFromGroup(syntaxElementToProcess.Parent, groupable);
                }
            }
        }
    }

    private void RemoveFromGroup(CompositeSyntaxElement group, IGroupableSemantic element)
    {
        foreach (var syntaxElement in element.GroupableSyntaxElements)
        {
            if (syntaxElement.Parent != null)
                syntaxElement.Parent.Make(syntaxElement).Deleted();
            
            group.Parent!.Make(syntaxElement).Before(group);
        }
    }
}