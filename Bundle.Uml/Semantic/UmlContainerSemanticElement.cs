using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Bundle.Uml.Semantic;

public class UmlContainerSemanticElement: ISemanticElement, IGroupSemantic
{
    private readonly UmlContainerSyntaxElement _syntaxElement;

    public UmlContainerSemanticElement(UmlContainerSyntaxElement syntaxElement)
    {
        _syntaxElement = syntaxElement;
    }

    public String Id => _syntaxElement.Name;
    
    public CompositeSyntaxElement GroupSyntaxElement  => _syntaxElement;
    
    public IReadOnlyCollection<SyntaxElement> SyntaxElements => new[] { _syntaxElement };
}