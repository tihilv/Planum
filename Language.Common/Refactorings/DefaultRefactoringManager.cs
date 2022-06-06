using Language.Api;
using Language.Api.Refactorings;
using Language.Api.Syntax;

namespace Language.Common.Refactorings;

public class DefaultRefactoringManager : IRefactoringManager
{
    private readonly IBundleBuilder _bundleBuilder;

    public DefaultRefactoringManager(IBundleBuilder bundleBuilder)
    {
        _bundleBuilder = bundleBuilder;
    }

    public void Refactor(RootSyntaxElement rootSyntaxElement)
    {
        var refactorings = _bundleBuilder.GetRefactorings().OrderBy(r=>r.Priority).ToArray();

        DoRefactor(rootSyntaxElement, refactorings);
    }

    private void DoRefactor(ICompositeSyntaxElement rootSyntaxElement, ISyntaxRefactoring[] refactorings)
    {
        foreach (var refactoring in refactorings)
            refactoring.Refactor(rootSyntaxElement);

        foreach (var child in rootSyntaxElement.Children)
            if (child is ICompositeSyntaxElement compositeChild)
                DoRefactor(compositeChild, refactorings);
    }
}