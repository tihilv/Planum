using Language.Api.Semantic;

namespace Language.Api.Operations;

public interface IOperationProcessor
{
    public IEnumerable<OperationDefinition> GetOperationsToExecute(ICollection<ISemanticElement> elements);
}