using Bundle.Uml.Elements;
using Language.Api;
using Language.Common;

namespace Bundle.Uml.Parsers;

public class StartUmlRootParser : SingleStatementParser<UmlRootSyntaxElement>
{
    public static readonly IParser Instance = new StartUmlRootParser();
    
    private StartUmlRootParser()
    {
        
    }

    protected override String ElementName => "@startuml";
    protected override ParseResult GetResult()
    {
        return new ParseResult(new UmlRootSyntaxElement(), UmlBundle.Name, EndUmlRootParser.Instance);
    }

    private class EndUmlRootParser : SingleStatementParser<UmlRootSyntaxElement>
    {
        internal static readonly IParser Instance = new EndUmlRootParser();
    
        private EndUmlRootParser()
        {
        
        }

        protected override String ElementName => "@enduml";
        protected override ParseResult GetResult()
        {
            return ParseResult.EmptyResult;
        }
    }
}