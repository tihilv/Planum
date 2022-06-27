using Language.Api;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Api.Transfers;
using Language.Common.Transfers;

namespace Language.Processing;

public class DocumentModel: IDocumentModel
{
    private readonly IBundleBuilder _bundleBuilder;

    private readonly IScript _script;
    private RootSyntaxElement _syntaxModel;
    private ISemanticElement[] _semanticModel;

    private readonly ScriptInterpreter _scriptInterpreter;
    private readonly ISemanticConverter _semanticConverter;

    public RootSyntaxElement SyntaxModel => _syntaxModel;

    public ISemanticElement[] SemanticModel => _semanticModel;

    public DocumentModel(IScript script, IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
        _script = script;

        _scriptInterpreter = new ScriptInterpreter(_script, bundleBuilder);
        _semanticConverter = new DefaultSemanticConverter(_bundleBuilder);
    }

    private void CalculateModel()
    {
        _scriptInterpreter.UpdateSyntaxModel();
        _syntaxModel = _scriptInterpreter.RootSyntaxElement;
        _semanticModel = _semanticConverter.GetSemanticElements(_syntaxModel).ToArray();
    }
}