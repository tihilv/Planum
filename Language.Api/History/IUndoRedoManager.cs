namespace Language.Api.History;

public class UndoRedoManager
{
    private readonly Stack<UndoableStep> _stepsToUndo;
    private readonly Stack<UndoableStep> _stepsToRedo;

    public UndoRedoManager()
    {
        _stepsToUndo = new Stack<UndoableStep>();
        _stepsToRedo = new Stack<UndoableStep>();
    }

    public event EventHandler<EventArgs> Changing;
    public event EventHandler<EventArgs> Changed;

    public IDisposable CreateTransaction()
    {
        return UndoableContext.Instance.CreateScope(RegisteringStep, RegisteredStep);
    }

    private void RegisteringStep(UndoableStep step)
    {
        OnChanging();
    }
    
    private void RegisteredStep(UndoableStep step)
    {
        _stepsToRedo.Clear();
        _stepsToUndo.Push(step);
        
        OnChanged();
    }

    public bool CanUndo() => _stepsToUndo.Any();

    public void Undo()
    {
        var step = _stepsToUndo.Pop();
        _stepsToRedo.Push(step);
        step.Undo();
        
        OnChanged();
    }

    public bool CanRedo() => _stepsToRedo.Any();
    public void Redo()
    {
        var step = _stepsToRedo.Pop();
        _stepsToUndo.Push(step);
        step.Do();
        
        OnChanged();
    }

    protected virtual void OnChanging()
    {
        Changing?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}