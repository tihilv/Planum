using System.Text;
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
        if (!((tokens.Length == 3 && tokens[2].Value == "{") || (tokens.Length == 4 && tokens[3].Value == "{")))
            return null;

        UmlContainerType containerType;
        if (!Enum.TryParse(typeof(UmlContainerType), tokens[0].Value, true, out var type))
            return null;

        containerType = (UmlContainerType)type;

        string? url = null;
        if (tokens.Length == 4 && tokens[2].Value.StartsWith("[[") && tokens[2].Value.EndsWith("]]"))
            url = tokens[2].Value.Substring(2, tokens[2].Value.Length - 4).Trim();
        
        return new ParseResult(new UmlContainerSyntaxElement(containerType, tokens[1].Value, url), UmlBundle.Name, EndParser.Instance);
    }

    public SynthesizeResult? Synthesize(SyntaxElement element)
    {
        if (element is UmlContainerSyntaxElement el)
        {
            var sb = new StringBuilder();
            sb.Append($"{el.Type.ToString().ToLower()} \"{el.Name}\" ");
            if (!string.IsNullOrEmpty(el.Url))
                sb.Append($"[[{el.Url}]] ");
            sb.Append('{');
            
            return new SynthesizeResult(sb.ToString(), new SynthesizeNewScopeResult(UmlBundle.Name, "}"));
        }

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