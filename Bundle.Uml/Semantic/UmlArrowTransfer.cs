using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Api.Syntax;

namespace Bundle.Uml.Semantic;

public class UmlArrowTransfer: IFinishableSyntaxToSemanticTransfer
{
    public static readonly ISyntaxToSemanticTransfer Instance = new UmlArrowTransfer();
    
    private UmlArrowTransfer()
    { }
    
    public IEnumerable<ISemanticElement> TryConvert(SyntaxElement syntaxElement, SyntaxToSemanticContext context)
    {
        if (syntaxElement is UmlSyntaxElement uml)
        {
            if (uml.Arrow != null)
            {
                yield return new ArrowSemanticElement(uml);
            }
        }
    }

    public Boolean Postprocess(ISemanticElement semanticElement, SyntaxToSemanticContext context)
    {
        if (semanticElement is ArrowSemanticElement arrowSemanticElement)
        {
            var contextData = context.GetData<UmlContextData>();
            
            arrowSemanticElement.RegisterSemanticElements(contextData.GetSemanticElement(arrowSemanticElement.SyntaxElement.FirstFigure.Text), contextData.GetSemanticElement(arrowSemanticElement.SyntaxElement.SecondFigure!.Value.Text));
        }
        return true;
    }
}