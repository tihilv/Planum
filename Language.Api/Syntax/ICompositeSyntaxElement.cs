namespace Language.Api.Syntax;

public interface ICompositeSyntaxElement
{
    IReadOnlyCollection<SyntaxElement> Children { get; }

    ICompositeOperation Make(SyntaxElement syntaxElement);
}