using Bundle.Uml.Elements;
using Bundle.Uml.Semantic;
using Language.Api.Semantic;
using Language.Api.Syntax;
using Language.Api.Transfers;

namespace Bundle.Uml.Transfers;

public class UmlContainerTransfer: ISyntaxToSemanticTransfer
{
    public static readonly ISyntaxToSemanticTransfer Instance = new UmlContainerTransfer();
    
    private UmlContainerTransfer()
    { }
    
    public IEnumerable<ISemanticElement> TryConvert(SyntaxElement syntaxElement, SyntaxToSemanticContext context)
    {
        if (syntaxElement is UmlContainerSyntaxElement uml)
        {
            yield return new UmlContainerSemanticElement(uml);
        }
    }
}