using Language.Api.History;

namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement
{
    private partial class CompositeModification
    {
        private abstract class LocateUndoRedoAction : IUndoRedoAction
        {
            protected readonly CompositeSyntaxElement _compositeSyntax;
            private readonly SyntaxElement _newSyntaxElement;
            private readonly LinkedListNode<SyntaxElement>? _existingNode;
            private LinkedListNode<SyntaxElement>? _newNode;
            
            // for undo of _existingNode: 
            private readonly LinkedListNode<SyntaxElement>? _previousNodeForExistingNode;
            private readonly LinkedListNode<SyntaxElement>? _nextNodeForExistingNode;

            protected LocateUndoRedoAction(CompositeSyntaxElement compositeSyntax, SyntaxElement newSyntaxElement, LinkedListNode<SyntaxElement>? existingNode)
            {
                _compositeSyntax = compositeSyntax;
                _newSyntaxElement = newSyntaxElement;
                _existingNode = existingNode;

                if (_existingNode != null)
                {
                    _previousNodeForExistingNode = _existingNode.Previous;
                    _nextNodeForExistingNode = _existingNode.Next;
                }
            }

            protected abstract void PlaceElement(LinkedListNode<SyntaxElement> node);
            protected abstract LinkedListNode<SyntaxElement> PlaceElement(SyntaxElement element);

            public void Do()
            {
                if (_existingNode != null)
                {
                    _compositeSyntax._children.Remove(_existingNode);
                    PlaceElement(_existingNode);
                }
                else
                {
                    if (_newNode == null)
                        _newNode = PlaceElement(_newSyntaxElement);
                    else
                        PlaceElement(_newNode);
                    
                    _compositeSyntax._childrenNodes.Add(_newSyntaxElement, _newNode);
                    _newSyntaxElement.SetParent(_compositeSyntax);
                }
            }

            public void Undo()
            {
                if (_existingNode != null)
                {
                    _compositeSyntax._children.Remove(_existingNode);
                    
                    if (_previousNodeForExistingNode == null)
                        _compositeSyntax._children.AddFirst(_existingNode);
                    else if (_nextNodeForExistingNode == null)
                        _compositeSyntax._children.AddLast(_existingNode);
                    else
                        _compositeSyntax._children.AddAfter(_previousNodeForExistingNode, _existingNode);
                }
                else
                {
                    _compositeSyntax._children.Remove(_newNode);
                    _compositeSyntax._childrenNodes.Remove(_newNode.ValueRef);
                    _newNode.ValueRef.SetParent(null);
                }
            }
        }

        private class FirstLocateUndoRedoAction : LocateUndoRedoAction
        {
            public FirstLocateUndoRedoAction(CompositeSyntaxElement compositeSyntax, SyntaxElement newSyntaxElement, LinkedListNode<SyntaxElement>? existingNode) : base(compositeSyntax, newSyntaxElement, existingNode)
            {
            }

            protected override void PlaceElement(LinkedListNode<SyntaxElement> node)
            {
                _compositeSyntax._children.AddFirst(node);
            }

            protected override LinkedListNode<SyntaxElement> PlaceElement(SyntaxElement element)
            {
                return _compositeSyntax._children.AddFirst(element);
            }
        }
        
        private class LastLocateUndoRedoAction : LocateUndoRedoAction
        {
            public LastLocateUndoRedoAction(CompositeSyntaxElement compositeSyntax, SyntaxElement newSyntaxElement, LinkedListNode<SyntaxElement>? existingNode) : base(compositeSyntax, newSyntaxElement, existingNode)
            {
            }

            protected override void PlaceElement(LinkedListNode<SyntaxElement> node)
            {
                _compositeSyntax._children.AddLast(node);
            }

            protected override LinkedListNode<SyntaxElement> PlaceElement(SyntaxElement element)
            {
                return _compositeSyntax._children.AddLast(element);
            }
        }
        
        private class BeforeLocateUndoRedoAction : LocateUndoRedoAction
        {
            private readonly LinkedListNode<SyntaxElement> _relativeNode;

            public BeforeLocateUndoRedoAction(CompositeSyntaxElement compositeSyntax, SyntaxElement newSyntaxElement, LinkedListNode<SyntaxElement> relativeNode, LinkedListNode<SyntaxElement>? existingNode) : base(compositeSyntax, newSyntaxElement, existingNode)
            {
                _relativeNode = relativeNode;
            }

            protected override void PlaceElement(LinkedListNode<SyntaxElement> node)
            {
                _compositeSyntax._children.AddBefore(_relativeNode, node);
            }

            protected override LinkedListNode<SyntaxElement> PlaceElement(SyntaxElement element)
            {
                return _compositeSyntax._children.AddBefore(_relativeNode, element);
            }
        }
        
        private class AfterLocateUndoRedoAction : LocateUndoRedoAction
        {
            private readonly LinkedListNode<SyntaxElement> _relativeNode;

            public AfterLocateUndoRedoAction(CompositeSyntaxElement compositeSyntax, SyntaxElement newSyntaxElement, LinkedListNode<SyntaxElement> relativeNode, LinkedListNode<SyntaxElement>? existingNode) : base(compositeSyntax, newSyntaxElement, existingNode)
            {
                _relativeNode = relativeNode;
            }

            protected override void PlaceElement(LinkedListNode<SyntaxElement> node)
            {
                _compositeSyntax._children.AddAfter(_relativeNode, node);
            }

            protected override LinkedListNode<SyntaxElement> PlaceElement(SyntaxElement element)
            {
                return _compositeSyntax._children.AddAfter(_relativeNode, element);
            }
        }
    }
}