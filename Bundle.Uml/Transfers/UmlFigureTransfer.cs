using Bundle.Uml.Elements;
using Bundle.Uml.Semantic;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Api.Transfers;

namespace Bundle.Uml.Transfers;

public class UmlFigureTransfer : IFinishableSyntaxToSemanticTransfer
{
    public static readonly ISyntaxToSemanticTransfer Instance = new UmlFigureTransfer();

    private UmlFigureTransfer()
    {
    }

    public IEnumerable<ISemanticElement> TryConvert(SyntaxElement syntaxElement, SyntaxToSemanticContext context)
    {
        if (syntaxElement is UmlSyntaxElement uml)
        {
            var contextData = context.GetData<UmlContextData>();

            var figure1 = contextData.Register(uml, uml.FirstFigure);
            if (figure1 != null)
                yield return figure1;
            if (uml.SecondFigure != null)
            {
                var figure2 = contextData.Register(uml, uml.SecondFigure.Value);
                if (figure2 != null)
                    yield return figure2;
            }
        }
    }

    public Boolean Postprocess(ISemanticElement semanticElement, SyntaxToSemanticContext context)
    {
        var contextData = context.GetData<UmlContextData>();

        return !contextData.IsDeleted(semanticElement);
    }
}