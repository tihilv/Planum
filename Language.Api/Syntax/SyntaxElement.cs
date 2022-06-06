namespace Language.Api.Syntax;

public abstract class SyntaxElement
{
    public CompositeSyntaxElement? Parent { get; private set; }

    internal void SetParent(CompositeSyntaxElement? parent)
    {
        if (parent != null && Parent != null && Parent != parent)
            throw new InvalidOperationException("The element is already attached to parent.");

        Parent = parent;
    }
}