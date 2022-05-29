namespace Language.Api.Syntax;

public abstract partial class CompositeSyntaxElement : SyntaxElement
{
    private readonly LinkedList<SyntaxElement> _children;

    protected CompositeSyntaxElement()
    {
        _children = new LinkedList<SyntaxElement>();
    }

    public IReadOnlyCollection<SyntaxElement> Children => _children;

    public ICompositeOperation Make(SyntaxElement syntaxElement)
    {
        return new CompositeSyntaxElement.CompositeOperation(this, syntaxElement);
    }
}