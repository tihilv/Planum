using Language.Api.History;

namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement
{
    private partial class CompositeModification
    {
        private class DeleteUndoRedoAction : IUndoRedoAction
        {
            private readonly CompositeSyntaxElement _compositeSyntax;
            private readonly LinkedListNode<SyntaxElement> _node;
            
            private readonly LinkedListNode<SyntaxElement>? _previousNode;
            private readonly LinkedListNode<SyntaxElement>? _nextNode;

            public DeleteUndoRedoAction(CompositeSyntaxElement compositeSyntax, LinkedListNode<SyntaxElement> node)
            {
                _compositeSyntax = compositeSyntax;
                _node = node;

                _previousNode = _node.Previous;
                _nextNode = _node.Next;
            }

            public void Do()
            {
                _compositeSyntax._children.Remove(_node);
                _compositeSyntax._childrenNodes.Remove(_node.ValueRef);
                _node.ValueRef.SetParent(null);
            }

            public void Undo()
            {
                if (_previousNode == null)
                    _compositeSyntax._children.AddFirst(_node);
                else if (_nextNode == null)
                    _compositeSyntax._children.AddLast(_node);
                else
                    _compositeSyntax._children.AddAfter(_previousNode, _node);
                
                _compositeSyntax._childrenNodes.Add(_node.ValueRef, _node);
                
                _node.ValueRef.SetParent(_compositeSyntax);
            }
        }
    }
}