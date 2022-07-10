namespace Language.Api.History;

public class UndoableContext
{
    public static readonly UndoableContext Instance = new UndoableContext();

    private readonly AsyncLocal<UndoableStep?> _currentStep;

    private UndoableContext()
    {
        _currentStep = new AsyncLocal<UndoableStep?>();
    }

    internal IDisposable CreateScope(Action<UndoableStep> registerAction)
    {
        return new Handle(this, new UndoableStep(), registerAction);
    }
    
    public void RegisterAction(IUndoRedoAction action)
    {
        _currentStep.Value?.RegisterAction(action);
    }

    private class Handle : IDisposable
    {
        private readonly UndoableContext _undoableContext;
        private readonly Action<UndoableStep> _registerAction;

        public Handle(UndoableContext undoableContext, UndoableStep step, Action<UndoableStep> registerAction)
        {
            if (undoableContext._currentStep.Value != null)
                throw new InvalidOperationException("Unable to create inner context.");
            
            _undoableContext = undoableContext;
            _registerAction = registerAction;
            _undoableContext._currentStep.Value = step;
        }

        public void Dispose()
        {
            var step = _undoableContext._currentStep.Value; 
            _undoableContext._currentStep.Value = null;
            if (step != null) 
                _registerAction(step);
        }
    }
    
}