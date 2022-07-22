using Language.Api;
using Language.Api.History;
using Language.Api.Operations;
using Language.Api.Refactorings;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Api.Transfers;
using Language.Common.Refactorings;
using Language.Common.Transfers;

namespace Language.Processing;

public class DocumentModel: IDocumentModel
{
    private readonly IBundleBuilder _bundleBuilder;

    private readonly IScript _script;
    private RootSyntaxElement _syntaxModel;
    private ISemanticElement[] _semanticModel;

    private readonly UndoRedoManager _undoRedoManager;
    
    private readonly ScriptInterpreter _scriptInterpreter;
    private readonly ISemanticConverter _semanticConverter;
    private readonly IRefactoringManager _refactoringManager;

    public IScript GetScript()
    {
        _scriptInterpreter.UpdateScript();
        return _script;
    }
    
    public RootSyntaxElement SyntaxModel => _syntaxModel;
    public ISemanticElement[] SemanticModel => _semanticModel;

    public event EventHandler<EventArgs> Changed; 

    public DocumentModel(IScript script, IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
        _script = script;

        _undoRedoManager = new UndoRedoManager();
        _undoRedoManager.Changed += OnUndoRedoChanged;
        
        _scriptInterpreter = new ScriptInterpreter(_script, bundleBuilder);
        _semanticConverter = new DefaultSemanticConverter(_bundleBuilder);
        _refactoringManager = new DefaultRefactoringManager(_bundleBuilder);

        CalculateModel();
    }

    private void OnUndoRedoChanged(Object? sender, EventArgs e)
    {
        CalculateModel();
    }

    private void CalculateModel()
    {
        _scriptInterpreter.UpdateSyntaxModel();
        _syntaxModel = _scriptInterpreter.RootSyntaxElement;
        _semanticModel = _semanticConverter.GetSemanticElements(_syntaxModel).ToArray();

        OnChanged();
    }

    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void ExecuteOperation(IOperation operation, IEnumerable<ISemanticElement> selectedElements, object[] arguments)
    {
        using (_undoRedoManager.CreateTransaction())
        {
            operation.Execute(SemanticModel, selectedElements, arguments);
            _refactoringManager.Refactor(SyntaxModel);
        }
    }
}