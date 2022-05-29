using Language.Api;
using Language.Api.Syntax;

namespace Language.Processing;

public class ScriptAssociations
{
    private readonly Dictionary<int, Tuple> _byLine;
    private readonly Dictionary<SyntaxElement, Tuple> _byElement;

    public ScriptAssociations()
    {
        _byLine = new Dictionary<int, Tuple>();
        _byElement = new Dictionary<SyntaxElement, Tuple>();
    }

    public void Register(ScriptLine line, IParser parser, SyntaxElement syntaxElement)
    {
        var tuple = new Tuple(line, parser, syntaxElement);
        _byLine.Add(line.Identifier, tuple);
        _byElement.Add(syntaxElement, tuple);
    }

    private struct Tuple
    {
        public readonly ScriptLine Line;
        public readonly IParser Parser;
        public readonly SyntaxElement SyntaxElement;

        public Tuple(ScriptLine line, IParser parser, SyntaxElement syntaxElement)
        {
            Line = line;
            Parser = parser;
            SyntaxElement = syntaxElement;
        }
    }
}