using Language.Api.Semantic;
using Language.Api.Syntax;

namespace Language.Api.Transfers;

public interface ISemanticConverter
{
    IEnumerable<ISemanticElement> GetSemanticElements(RootSyntaxElement rootSyntaxElement);
}