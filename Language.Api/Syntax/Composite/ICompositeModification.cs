namespace Language.Api.Syntax;

public interface ICompositeModification
{
    void First();
    void Last();
    void Before(SyntaxElement relativeSyntaxElement);
    void After(SyntaxElement relativeSyntaxElement);
    void Replacing(SyntaxElement oldSyntaxElement);
    void Deleted();
}