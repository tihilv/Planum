using Language.Api.Syntax;

namespace Language.Common.Semantic;

public interface IGroupableSemantic
{
    public IReadOnlyCollection<SyntaxElement> GroupableSyntaxElements { get; }
}