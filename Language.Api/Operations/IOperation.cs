using Language.Api.Semantic;

namespace Language.Api.Operations;

public interface IOperation
{
    public OperationDefinition Definition { get; }

    public bool CanExecute(IEnumerable<ISemanticElement> elements);

    public void Execute(ISemanticElement[] allElements, IEnumerable<ISemanticElement> selectedElements, object[] arguments);
}