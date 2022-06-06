using Language.Api.Syntax;

namespace Language.Api.Refactorings;

public interface ISyntaxRefactoring
{
    byte Priority { get; }
    void Refactor(ICompositeSyntaxElement compositeSyntaxElement);
}