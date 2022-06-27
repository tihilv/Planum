using Bundle.Uml.Elements;
using Language.Api;
using Language.Api.Syntax;
using Language.Common;

namespace Bundle.Uml.Parsers;

public class UmlContainerElementParser : IParser
{
    public static readonly IParser Instance = new UmlContainerElementParser();

    private UmlContainerElementParser()
    {
    }

    public ParseResult? Parse(Token[] tokens)
    {
        if (tokens.Length != 3)
            return null;

        if (tokens[2].Value != "{")
            return null;

        UmlContainerType containerType;
        if (!Enum.TryParse(typeof(UmlContainerType), tokens[0].Value, true, out var type))
            return null;

        containerType = (UmlContainerType)type;

        return new ParseResult(new UmlContainerSyntaxElement(containerType, tokens[1].Value), UmlBundle.Name, EndParser.Instance);
    }

    public SynthesizeResult? Synthesize(SyntaxElement element)
    {
        if (element is UmlContainerSyntaxElement el)
            return new SynthesizeResult($"{el.Type} {el.Name} {{", new SynthesizeNewScopeResult(UmlBundle.Name, "}"));

        return null;
    }

    private class EndParser : SingleStatementParser<UmlContainerSyntaxElement>
    {
        internal static readonly IParser Instance = new EndParser();
    
        private EndParser()
        { }

        protected override String ElementName => "}";
        protected override ParseResult GetResult()
        {
            return ParseResult.EmptyResult;
        }
    }
}