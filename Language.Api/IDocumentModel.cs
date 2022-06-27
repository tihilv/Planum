using Language.Api.Semantic;
using Language.Api.Syntax;

namespace Language.Api;

public interface IDocumentModel
{
    RootSyntaxElement SyntaxModel { get; }
    ISemanticElement[] SemanticModel { get; }
}