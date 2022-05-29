using Language.Api.Syntax;

namespace Language.Api;

public struct ParseResult
{
    public static readonly ParseResult EmptyResult = new ParseResult();
    
    public readonly SyntaxElement SyntaxElement;
    public readonly NewScopeResult? NewScopeResult;

    public ParseResult(SyntaxElement syntaxElement)
    {
        SyntaxElement = syntaxElement;
        NewScopeResult = null;
    }

    public ParseResult(CompositeSyntaxElement syntaxElement, string newScopeName, IParser finalParser): this(syntaxElement)
    {
        NewScopeResult = new NewScopeResult(newScopeName, finalParser);
    }
}

public struct NewScopeResult
{
    public readonly string Name;
    public readonly IParser FinalParser;

    public NewScopeResult(String name, IParser finalParser)
    {
        Name = name;
        FinalParser = finalParser;
    }
}