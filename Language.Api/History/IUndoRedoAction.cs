namespace Language.Api.History;

public interface IUndoRedoAction
{
    void Do();
    void Undo();
}