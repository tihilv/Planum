using Language.Api.Semantic;

namespace Language.Api.Operations;

public interface IPropertyOperation : IOperation
{
    public Type PropertyType { get; }
    public object GetValue(ISemanticElement element);
}