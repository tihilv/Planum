using Language.Api;
using Language.Api.Operations;
using Language.Api.Semantic;

namespace Language.Common.Operations;

public class DefaultOperationProcessor: IOperationProcessor
{
    private readonly IBundleBuilder _bundleBuilder;

    public DefaultOperationProcessor(IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
    }

    public IEnumerable<IOperation> GetOperationsToExecute(ICollection<ISemanticElement> elements)
    {
        foreach (var tuple in _bundleBuilder.GetOperations())
        {
            foreach (var operation in tuple.Value)
            {
                if (operation.CanExecute(elements))
                {
                    yield return operation;
                    break;
                }
            }
        }
    }
}