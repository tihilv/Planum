using Language.Api;
using Language.Api.Syntax;

namespace Language.Processing;

public class ScriptScope
{
    public readonly ITokenizer Tokenizer;
    public readonly IReadOnlyCollection<IParser> Parsers;

    public readonly CompositeSyntaxElement CurrentSyntaxElement;

    public readonly IParser? FinalParser;

    public ScriptScope(ITokenizer tokenizer, IReadOnlyCollection<IParser> parsers, CompositeSyntaxElement currentSyntaxElement, IParser? finalParser)
    {
        Tokenizer = tokenizer;
        Parsers = parsers;
        FinalParser = finalParser;
        CurrentSyntaxElement = currentSyntaxElement;
    }
}