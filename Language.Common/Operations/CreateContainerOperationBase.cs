using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Language.Common.Operations;

public abstract class CreateContainerOperationBase: IOperation
{
    public abstract OperationDefinition Definition { get; }

    public Boolean CanExecute(IEnumerable<ISemanticElement> elements)
    {
        return elements.Where(e => e is IGroupableSemantic).SelectMany(e => e.SyntaxElements).Select(e => e.Parent).Distinct().Count() == 1;
    }

    protected abstract CompositeSyntaxElement CreateContainer(Object[] arguments);
    
    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, Object[] arguments)
    {
        var containerSyntax = CreateContainer(arguments);

        var semanticElementsToProcess = selectedElements.Where(e=>e is IGroupableSemantic).ToArray();

        var firstSyntaxElement = semanticElementsToProcess.SelectMany(e => e.SyntaxElements).First();
        firstSyntaxElement.Parent!.Make(containerSyntax).Before(firstSyntaxElement);

        foreach (var groupable in semanticElementsToProcess.OfType<IGroupableSemantic>())
        {
            var syntaxElements = groupable.GroupableSyntaxElements;

            foreach (var syntaxElement in syntaxElements)
            {
                if (syntaxElement.Parent != null)
                    syntaxElement.Parent.Make(syntaxElement).Deleted();
                
                containerSyntax.Make(syntaxElement).Last();
            }
        }
    }
}