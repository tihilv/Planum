using Language.Api;
using Language.Api.Operations;
using Language.Api.Semantic;

namespace Language.Common.Semantic;

public class DefaultOperationProcessor: IOperationProcessor
{
    private readonly IBundleBuilder _bundleBuilder;

    public DefaultOperationProcessor(IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
    }

    public IEnumerable<OperationDefinition> GetOperationsToExecute(ICollection<ISemanticElement> elements)
    {
        foreach (var tuple in _bundleBuilder.GetOperations())
        {
            foreach (var operation in tuple.Value)
            {
                if (operation.CanExecute(elements))
                {
                    yield return operation.Definition;
                    break;
                }
            }
        }
    }
}