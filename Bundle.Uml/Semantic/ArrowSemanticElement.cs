using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Common.Primitives;

namespace Bundle.Uml.Semantic;

public class ArrowSemanticElement: ISemanticElement
{
    private string _id;
    
    private readonly UmlSyntaxElement _arrowUmlSyntaxElement;

    private UmlFigureSemanticElement _firstFigureSemanticElement;
    private UmlFigureSemanticElement _secondFigureSemanticElement;

    public ArrowSemanticElement(UmlSyntaxElement arrowUmlSyntaxElement)
    {
        _arrowUmlSyntaxElement = arrowUmlSyntaxElement;
        _id = $"{arrowUmlSyntaxElement.FirstFigure.Text}_{arrowUmlSyntaxElement.SecondFigure!.Value.Text}";
    }

    public String Id => _id;
    
    public Arrow Arrow => _arrowUmlSyntaxElement.Arrow!.Value;
    public UmlSyntaxElement SyntaxElement => _arrowUmlSyntaxElement;
    public UmlFigureSemanticElement FirstFigureSemanticElement => _firstFigureSemanticElement;
    public UmlFigureSemanticElement SecondFigureSemanticElement => _secondFigureSemanticElement;

    internal void RegisterSemanticElements(UmlFigureSemanticElement firstFigureSemanticElement, UmlFigureSemanticElement secondFigureSemanticElement)
    {
        _firstFigureSemanticElement = firstFigureSemanticElement;
        _secondFigureSemanticElement = secondFigureSemanticElement;
        
        _id = $"{_firstFigureSemanticElement.Alias??_firstFigureSemanticElement.Text}_{_secondFigureSemanticElement.Alias??_secondFigureSemanticElement.Text}";
    }
}