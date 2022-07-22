namespace Language.Api.History;

public class UndoableContext
{
    public static readonly UndoableContext Instance = new UndoableContext();

    private readonly AsyncLocal<UndoableStep?> _currentStep;

    private UndoableContext()
    {
        _currentStep = new AsyncLocal<UndoableStep?>();
    }

    internal IDisposable CreateScope(Action<UndoableStep> registeringAction, Action<UndoableStep> registeredAction)
    {
        return new Handle(this, new UndoableStep(), registeringAction, registeredAction);
    }
    
    public void RegisterAction(IUndoRedoAction action)
    {
        _currentStep.Value?.RegisterAction(action);
    }

    private class Handle : IDisposable
    {
        private readonly UndoableContext _undoableContext;
        private readonly Action<UndoableStep> _registeringAction; 
        private readonly Action<UndoableStep> _registeredAction;

        public Handle(UndoableContext undoableContext, UndoableStep step, Action<UndoableStep> registeringAction, Action<UndoableStep> registeredAction)
        {
            if (undoableContext._currentStep.Value != null)
                throw new InvalidOperationException("Unable to create inner context.");
            
            _undoableContext = undoableContext;
            _registeringAction = registeringAction;
            _registeredAction = registeredAction;
            _undoableContext._currentStep.Value = step;
        }

        public void Dispose()
        {
            var step = _undoableContext._currentStep.Value; 
            if (step != null) 
                _registeringAction(step);
            _undoableContext._currentStep.Value = null;
            if (step != null) 
                _registeredAction(step);
        }
    }
    
}