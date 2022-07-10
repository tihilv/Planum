using Language.Api.History;

namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement
{
    private partial class CompositeModification
    {
        private class ReplaceUndoRedoAction : IUndoRedoAction
        {
            private readonly CompositeSyntaxElement _compositeSyntax;
            private readonly SyntaxElement _oldSyntaxElement;
            private readonly SyntaxElement _newSyntaxElement;
            private readonly LinkedListNode<SyntaxElement> _node;

            public ReplaceUndoRedoAction(CompositeSyntaxElement compositeSyntax, LinkedListNode<SyntaxElement> node, SyntaxElement newSyntaxElement)
            {
                _compositeSyntax = compositeSyntax;
                _newSyntaxElement = newSyntaxElement;
                _node = node;
                _oldSyntaxElement = _node.ValueRef;
            }

            public void Do()
            {
                Do(_newSyntaxElement);
            }

            public void Undo()
            {
                Do(_oldSyntaxElement);
            }


            private void Do(SyntaxElement syntaxElement)
            {
                _compositeSyntax._childrenNodes.Remove(_node.ValueRef);
                _node.ValueRef = syntaxElement;
                _compositeSyntax._childrenNodes.Add(_node.ValueRef, _node);

                syntaxElement.SetParent(_compositeSyntax);
            }
        }
    }
}