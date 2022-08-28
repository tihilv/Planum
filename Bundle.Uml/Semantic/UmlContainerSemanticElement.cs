using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Semantic;

namespace Bundle.Uml.Semantic;

public class UmlContainerSemanticElement: ISemanticElement, IGroupSemantic, IUrlSemantic, ITextedSemantic
{
    private UmlContainerSyntaxElement _syntaxElement;

    public UmlContainerSemanticElement(UmlContainerSyntaxElement syntaxElement)
    {
        _syntaxElement = syntaxElement;
    }

    public String Id => _syntaxElement.Name;
    
    public CompositeSyntaxElement GroupSyntaxElement  => _syntaxElement;
    
    public IReadOnlyCollection<SyntaxElement> SyntaxElements => new[] { _syntaxElement };
    public ISemanticElement GetSnapshot()
    {
        return new UmlContainerSemanticElement(_syntaxElement);
    }

    public String? Url
    {
        get { return _syntaxElement.Url; }
        set => _syntaxElement.Parent!.Make(_syntaxElement.With(value)).Replacing(_syntaxElement);
    }

    public String Text
    {
        get { return _syntaxElement.Name; }
        set { SetText(value); }
    }

    private void SetText(String text)
    {
        var newElement = _syntaxElement.With(name: text);
        _syntaxElement.Parent!.Make(newElement).Replacing(_syntaxElement);
        _syntaxElement = newElement;
    }
}