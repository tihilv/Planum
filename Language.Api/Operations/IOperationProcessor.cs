using Language.Api.Semantic;

namespace Language.Api.Operations;

public interface IOperationProcessor
{
    public IEnumerable<IOperation> GetOperationsToExecute(ICollection<ISemanticElement> elements);
}