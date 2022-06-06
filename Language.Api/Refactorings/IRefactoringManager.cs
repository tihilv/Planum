using Language.Api.Syntax;

namespace Language.Api.Refactorings;

public interface IRefactoringManager
{
    void Refactor(RootSyntaxElement rootSyntaxElement);
}