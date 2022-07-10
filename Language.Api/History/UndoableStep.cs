namespace Language.Api.History;

public class UndoableStep: IUndoRedoAction
{
    private bool _undone;
    private readonly List<IUndoRedoAction> _actions;

    public UndoableStep()
    {
        _actions = new List<IUndoRedoAction>();
    }

    public void RegisterAction(IUndoRedoAction action)
    {
        if (_undone)
            throw new InvalidOperationException("Can't add new actions for a step in undone state.");

        _actions.Add(action);
    }

    public void Undo()
    {
        if (_undone)
            throw new InvalidOperationException("The step is already undone.");

        _undone = true;
        for (var index = _actions.Count-1; index >=0; index--)
            _actions[index].Undo();
    }

    public void Do()
    {
        if (!_undone)
            throw new InvalidOperationException("The step is already applied.");

        _undone = true;
        for (var index = 0; index < _actions.Count; index++)
            _actions[index].Do();
    }

}