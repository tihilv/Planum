using Bundle.Uml.Elements;
using Bundle.Uml.Semantic;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Api.Transfers;

namespace Bundle.Uml.Transfers;

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
            
            arrowSemanticElement.RegisterSemanticElements(contextData.GetSemanticElement(arrowSemanticElement.SyntaxElement.FirstFigure), contextData.GetSemanticElement(arrowSemanticElement.SyntaxElement.SecondFigure!.Value));
        }
        return true;
    }
}