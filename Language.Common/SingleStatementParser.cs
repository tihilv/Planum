using Language.Api;
using Language.Api.Syntax;

namespace Language.Common;

public abstract class SingleStatementParser<T>: IParser where T:CompositeSyntaxElement
{
    protected abstract string ElementName { get; }

    protected abstract ParseResult GetResult();

    
    public ParseResult? Parse(Token[] tokens)
    {
        if (tokens.Length != 1)
            return null;

        var token = tokens[0];
        if (token.Value.Trim().Equals(ElementName, StringComparison.InvariantCultureIgnoreCase))
            return GetResult();

        return null;
    }

    public SynthesizeResult? Synthesize(SyntaxElement element)
    {
        if (element is T el)
        {
            var result = GetResult();
            return new SynthesizeResult(ElementName, new SynthesizeNewScopeResult(result.NewScopeResult?.Name, (result.NewScopeResult?.FinalParser as SingleStatementParser<T>)?.ElementName));
        }

        return null;
    }
}