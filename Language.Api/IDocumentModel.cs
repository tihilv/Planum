using Language.Api.Operations;
using Language.Api.Semantic;
using Language.Api.Syntax;

namespace Language.Api;

public interface IDocumentModel
{
    IScript GetScript();
    RootSyntaxElement SyntaxModel { get; }
    ISemanticElement[] SemanticModel { get; }
    event EventHandler<EventArgs> Changed;
    void ExecuteOperation(IOperation operation, IEnumerable<ISemanticElement> selectedElements, object[] arguments);
}