using Language.Api.Syntax;

namespace Language.Api;

public struct ParseResult
{
    public static readonly ParseResult EmptyResult = new ParseResult();
    
    public readonly SyntaxElement SyntaxElement;
    public readonly ParseNewScopeResult? NewScopeResult;

    public ParseResult(SyntaxElement syntaxElement)
    {
        SyntaxElement = syntaxElement;
        NewScopeResult = null;
    }

    public ParseResult(CompositeSyntaxElement syntaxElement, string newScopeName, IParser finalParser): this(syntaxElement)
    {
        NewScopeResult = new ParseNewScopeResult(newScopeName, finalParser);
    }
}

public struct ParseNewScopeResult
{
    public readonly string Name;
    public readonly IParser FinalParser;

    public ParseNewScopeResult(String name, IParser finalParser)
    {
        Name = name;
        FinalParser = finalParser;
    }
}

public struct SynthesizeResult
{
    public readonly string Text;

    public readonly SynthesizeNewScopeResult? NewScopeResult;

    public SynthesizeResult(String text, SynthesizeNewScopeResult? newScopeResult = null)
    {
        Text = text;
        NewScopeResult = newScopeResult;
    }
}

public struct SynthesizeNewScopeResult
{
    public readonly string? Name;
    public readonly string FinalText;

    public SynthesizeNewScopeResult(String? name, String finalText)
    {
        Name = name;
        FinalText = finalText;
    }
}