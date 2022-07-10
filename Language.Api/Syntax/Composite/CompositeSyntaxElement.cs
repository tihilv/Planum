namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement : SyntaxElement, ICompositeSyntaxElement
{
    private readonly LinkedList<SyntaxElement> _children;
    private readonly Dictionary<SyntaxElement, LinkedListNode<SyntaxElement>> _childrenNodes;

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
}