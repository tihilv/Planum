using Language.Api.Syntax;

namespace Language.Api.Semantic;

public interface ISyntaxToSemanticTransfer
{
    IEnumerable<ISemanticElement> TryConvert(SyntaxElement syntaxElement, SyntaxToSemanticContext context);
}

public interface IFinishableSyntaxToSemanticTransfer: ISyntaxToSemanticTransfer
{
    Boolean Postprocess(ISemanticElement semanticElement, SyntaxToSemanticContext context);
}