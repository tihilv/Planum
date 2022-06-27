using Language.Api.Syntax;

namespace Language.Api;

public interface IParser
{
    ParseResult? Parse(Token[] tokens);

    SynthesizeResult? Synthesize(SyntaxElement element);
}