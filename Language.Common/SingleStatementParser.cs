using Language.Api;

namespace Language.Common;

public abstract class SingleStatementParser: IParser
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
}