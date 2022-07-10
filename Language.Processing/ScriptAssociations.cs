using Language.Api;
using Language.Api.Syntax;

namespace Language.Processing;

public class ScriptAssociations
{
    private readonly Dictionary<int, Tuple> _byLine;
    private readonly Dictionary<SyntaxElement, Tuple> _byElement;
    
    private readonly Dictionary<SyntaxElement, ScriptLine> _finalizations;

    public ScriptAssociations()
    {
        _byLine = new Dictionary<int, Tuple>();
        _byElement = new Dictionary<SyntaxElement, Tuple>();
        _finalizations = new Dictionary<SyntaxElement, ScriptLine>();
    }

    public void Register(ScriptLine line, IParser parser, SyntaxElement syntaxElement, String? newScopeName)
    {
        var tuple = new Tuple(line, parser, syntaxElement, newScopeName);
        _byLine.Add(line.Identifier, tuple);
        _byElement.Add(syntaxElement, tuple);
    }

    public void RegisterFinalResult(ScriptLine line, CompositeSyntaxElement syntaxElement)
    {
        _finalizations.Add(syntaxElement, line);
    }

    public (ScriptLine, SynthesizeNewScopeResult?)? FindScriptLines(SyntaxElement element)
    {
        if (_byElement.TryGetValue(element, out var result))
        {
            SynthesizeNewScopeResult? synthesizeNewScopeResult = null;
            if (_finalizations.TryGetValue(element, out var finalLine))
                synthesizeNewScopeResult = new SynthesizeNewScopeResult(result.NewScopeName, finalLine.Value);

            return (result.Line, synthesizeNewScopeResult);
        }

        return null;
    }

    private struct Tuple
    {
        public readonly ScriptLine Line;
        public readonly IParser Parser;
        public readonly SyntaxElement SyntaxElement;
        public readonly String? NewScopeName;

        public Tuple(ScriptLine line, IParser parser, SyntaxElement syntaxElement, String? newScopeName)
        {
            NewScopeName = newScopeName;
            Line = line;
            Parser = parser;
            SyntaxElement = syntaxElement;
        }
    }
}