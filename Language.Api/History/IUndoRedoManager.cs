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

    public IDisposable CreateTransaction()
    {
        return UndoableContext.Instance.CreateScope(RegisterStep);
    }

    private void RegisterStep(UndoableStep step)
    {
        _stepsToRedo.Clear();
        _stepsToUndo.Push(step);
    }

    public bool CanUndo() => _stepsToUndo.Any();

    public void Undo()
    {
        var step = _stepsToUndo.Pop();
        _stepsToRedo.Push(step);
        step.Undo();
    }

    public bool CanRedo() => _stepsToRedo.Any();
    public void Redo()
    {
        var step = _stepsToRedo.Pop();
        _stepsToUndo.Push(step);
        step.Do();
    }
}