using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Common.Primitives;
using Language.Common.Semantic;

namespace Bundle.Uml.Semantic;

public class ArrowSemanticElement: ISemanticElement, IArrowedSemantic
{
    private string _id;
    
    private UmlSyntaxElement _arrowUmlSyntaxElement;

    private ISemanticElement _firstSemanticElement;
    private ISemanticElement _secondSemanticElement;

    public ArrowSemanticElement(UmlSyntaxElement arrowUmlSyntaxElement)
    {
        _arrowUmlSyntaxElement = arrowUmlSyntaxElement;
        _id = $"{arrowUmlSyntaxElement.FirstFigure.Text}_{arrowUmlSyntaxElement.SecondFigure!.Value.Text}";
    }

    public String Id => _id;

    public Arrow Arrow
    {
        get { return _arrowUmlSyntaxElement.Arrow!.Value; }
        set { SetArrow(value); }
    }

    private void SetArrow(Arrow value)
    {
        var newSyntaxElement = _arrowUmlSyntaxElement.With(arrow: value);
        _arrowUmlSyntaxElement.Parent!.Make(newSyntaxElement).Replacing(_arrowUmlSyntaxElement);
        _arrowUmlSyntaxElement = newSyntaxElement;
    }

    public UmlSyntaxElement SyntaxElement => _arrowUmlSyntaxElement;
    public ISemanticElement FirstSemanticElement => _firstSemanticElement;
    public ISemanticElement SecondSemanticElement => _secondSemanticElement;

    public IReadOnlyCollection<SyntaxElement> SyntaxElements => new[] { SyntaxElement };
    public ISemanticElement GetSnapshot()
    {
        return new ArrowSemanticElement(_arrowUmlSyntaxElement)
        {
            _id = _id,
            _firstSemanticElement = _firstSemanticElement,
            _secondSemanticElement = _secondSemanticElement,
        };
    }

    internal void RegisterSemanticElements(ISemanticElement firstSemanticElement, ISemanticElement secondSemanticElement)
    {
        _firstSemanticElement = firstSemanticElement;
        _secondSemanticElement = secondSemanticElement;
        
        _id = $"{_firstSemanticElement.Id}_{_secondSemanticElement.Id}";
    }
}