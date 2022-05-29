namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement
{
    private class CompositeOperation : ICompositeOperation
    {
        private readonly CompositeSyntaxElement _compositeSyntax;
        private readonly SyntaxElement _syntaxElement;
        private readonly LinkedListNode<SyntaxElement>? _existingNode;

        public CompositeOperation(CompositeSyntaxElement compositeSyntax, SyntaxElement syntaxElement)
        {
            _compositeSyntax = compositeSyntax;
            _syntaxElement = syntaxElement;

            _existingNode = _compositeSyntax._children.FindLast(_syntaxElement);
        }

        public void First()
        {
            if (_existingNode != null)
            {
                _compositeSyntax._children.Remove(_existingNode);
                _compositeSyntax._children.AddFirst(_existingNode);
            }
            else
                _compositeSyntax._children.AddFirst(_syntaxElement);
            
            _syntaxElement.SetParent(_compositeSyntax);
        }

        public void Last()
        {
            if (_existingNode != null)
            {
                _compositeSyntax._children.Remove(_existingNode);
                _compositeSyntax._children.AddLast(_existingNode);
            }
            else
                _compositeSyntax._children.AddLast(_syntaxElement);
            
            _syntaxElement.SetParent(_compositeSyntax);
        }

        public void Before(SyntaxElement relativeSyntaxElement)
        {
            var node = _compositeSyntax._children.FindLast(relativeSyntaxElement);
            if (node == null)
                throw new InvalidOperationException("Given relative element is not found.");

            if (_existingNode != null)
            {
                _compositeSyntax._children.Remove(_existingNode);
                _compositeSyntax._children.AddBefore(node, _existingNode);
            }
            else
                _compositeSyntax._children.AddBefore(node, _syntaxElement);
            
            _syntaxElement.SetParent(_compositeSyntax);
        }

        public void After(SyntaxElement relativeSyntaxElement)
        {
            var node = _compositeSyntax._children.FindLast(relativeSyntaxElement);
            if (node == null)
                throw new InvalidOperationException("Given relative element is not found.");

            if (_existingNode != null)
            {
                _compositeSyntax._children.Remove(_existingNode);
                _compositeSyntax._children.AddAfter(node, _existingNode);
            }
            else
                _compositeSyntax._children.AddAfter(node, _syntaxElement);
            
            _syntaxElement.SetParent(_compositeSyntax);
        }

        public void Deleted()
        {
            if (_existingNode != null)
                _compositeSyntax._children.Remove(_existingNode);
            else
                throw new InvalidOperationException("Element is not present in the collection.");
            
            _syntaxElement.SetParent(null);
        }
        
        public void Replacing(SyntaxElement oldSyntaxElement)
        {
            var node = _compositeSyntax._children.FindLast(oldSyntaxElement);
            if (node == null)
                throw new InvalidOperationException("Given old element is not found.");

            node.ValueRef = _syntaxElement;
            
            _syntaxElement.SetParent(_compositeSyntax);
        }
    }
}