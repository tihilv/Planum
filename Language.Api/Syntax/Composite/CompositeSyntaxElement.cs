namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement : SyntaxElement, ICompositeSyntaxElement
{
    private LinkedList<SyntaxElement> _children;
    private Dictionary<SyntaxElement, LinkedListNode<SyntaxElement>> _childrenNodes;

    protected CompositeSyntaxElement()
    {
        _children = new LinkedList<SyntaxElement>();
        _childrenNodes = new Dictionary<SyntaxElement, LinkedListNode<SyntaxElement>>();
    }

    public IReadOnlyCollection<SyntaxElement> Children => _children;

    public ICompositeModification Make(SyntaxElement syntaxElement)
    {
        return new CompositeModification(this, syntaxElement);
    }
    
    protected T With<T>(T oldElement, T newElement) where T: CompositeSyntaxElement
    {
        newElement._children = _children;
        newElement._childrenNodes = _childrenNodes;

        return newElement;
    }
}