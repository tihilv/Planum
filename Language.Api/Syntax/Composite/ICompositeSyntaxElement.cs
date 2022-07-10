namespace Language.Api.Syntax;

public interface ICompositeSyntaxElement
{
    IReadOnlyCollection<SyntaxElement> Children { get; }

    ICompositeModification Make(SyntaxElement syntaxElement);
}