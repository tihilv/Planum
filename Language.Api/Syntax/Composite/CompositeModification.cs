using Language.Api.History;

namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement
{
    private partial class CompositeModification : ICompositeModification
    {
        private readonly CompositeSyntaxElement _compositeSyntax;
        private readonly SyntaxElement _syntaxElement;
        private readonly LinkedListNode<SyntaxElement>? _existingNode;

        public CompositeModification(CompositeSyntaxElement compositeSyntax, SyntaxElement syntaxElement)
        {
            _compositeSyntax = compositeSyntax;
            _syntaxElement = syntaxElement;

            _compositeSyntax._childrenNodes.TryGetValue(_syntaxElement, out _existingNode);
        }

        public void First()
        {
            var action = new FirstLocateUndoRedoAction(_compositeSyntax, _syntaxElement, _existingNode);
            UndoableContext.Instance.RegisterAction(action);
            action.Do();
        }

        public void Last()
        {
            var action = new LastLocateUndoRedoAction(_compositeSyntax, _syntaxElement, _existingNode);
            UndoableContext.Instance.RegisterAction(action);
            action.Do();
        }

        public void Before(SyntaxElement relativeSyntaxElement)
        {
            if (!_compositeSyntax._childrenNodes.TryGetValue(relativeSyntaxElement, out var node))
                throw new InvalidOperationException("Given relative element is not found.");

            var action = new BeforeLocateUndoRedoAction(_compositeSyntax, _syntaxElement, node, _existingNode);
            UndoableContext.Instance.RegisterAction(action);
            action.Do();
        }

        public void After(SyntaxElement relativeSyntaxElement)
        {
            if (!_compositeSyntax._childrenNodes.TryGetValue(relativeSyntaxElement, out var node))
                throw new InvalidOperationException("Given relative element is not found.");

            var action = new AfterLocateUndoRedoAction(_compositeSyntax, _syntaxElement, node, _existingNode);
            UndoableContext.Instance.RegisterAction(action);
            action.Do();
        }

        public void Deleted()
        {
            if (_existingNode != null)
            {
                var action = new DeleteUndoRedoAction(_compositeSyntax, _existingNode);
                UndoableContext.Instance.RegisterAction(action);
                action.Do();
            }
            else
                throw new InvalidOperationException("Element is not present in the collection.");
        }
        
        public void Replacing(SyntaxElement oldSyntaxElement)
        {
            if (!_compositeSyntax._childrenNodes.TryGetValue(oldSyntaxElement, out var node))
                throw new InvalidOperationException("Given old element is not found.");

            var action = new ReplaceUndoRedoAction(_compositeSyntax, node, _syntaxElement);
            UndoableContext.Instance.RegisterAction(action);
            action.Do();
        }
    }
}