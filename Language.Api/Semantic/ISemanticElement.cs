using Language.Api.Syntax;

namespace Language.Api.Semantic;

public interface ISemanticElement
{
    public string Id { get; }
    
    public IReadOnlyCollection<SyntaxElement> SyntaxElements { get; }

    public ISemanticElement GetSnapshot();
}