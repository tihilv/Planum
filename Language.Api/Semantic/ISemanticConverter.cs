using Language.Api.Syntax;

namespace Language.Api.Semantic;

public interface ISemanticConverter
{
    IEnumerable<ISemanticElement> GetSemanticElements(RootSyntaxElement rootSyntaxElement);
}