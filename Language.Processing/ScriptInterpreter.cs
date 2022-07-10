using Language.Api;
using Language.Api.Exceptions;
using Language.Api.Syntax;

namespace Language.Processing;

public class ScriptInterpreter
{
    private readonly IScript _script;
    private readonly IBundleBuilder _bundleBuilder;

    private readonly ScriptAssociations _associations;
    
    public RootSyntaxElement RootSyntaxElement { get; private set; }

    public ScriptInterpreter(IScript script, IBundleBuilder bundleBuilder)
    {
        _script = script;
        _bundleBuilder = bundleBuilder;

        _associations = new ScriptAssociations();
    }

    public void UpdateSyntaxModel()
    {
        var scopeStack = new Stack<ScriptScope>();
        var scope = GetRootScriptScope();
        RootSyntaxElement = (RootSyntaxElement)scope.CurrentSyntaxElement;
        
        foreach (var line in _script.GetLines())
        {
            if (!string.IsNullOrEmpty(line.Value))
            {
                var tokens = scope.Tokenizer.GetTokens(line).ToArray();

                if (scope.FinalParser != null)
                {
                    var finalResult = scope.FinalParser.Parse(tokens);
                    if (finalResult != null)
                    {
                        if (!finalResult.Equals(ParseResult.EmptyResult))
                            throw new LineException(line, $"Unexpected answer from the final parser '{scope.FinalParser.GetType().Name}'");

                        _associations.RegisterFinalResult(line, scope.CurrentSyntaxElement);
                        
                        scope = scopeStack.Pop();
                        continue;
                    }
                }

                foreach (var parser in scope.Parsers)
                {
                    var result = parser.Parse(tokens);
                    if (result != null)
                    {
                        var element = result.Value.SyntaxElement;
                        scope.CurrentSyntaxElement.Make(element).Last();
                        _associations.Register(line, parser, element, result.Value.NewScopeResult?.Name);

                        if (result.Value.NewScopeResult != null)
                        {
                            scopeStack.Push(scope);
                            var newResult = result.Value.NewScopeResult.Value;

                            var tokenizer = _bundleBuilder.GetTokenizer(newResult.Name);
                            var parsers = _bundleBuilder.GetParsers(newResult.Name);
                            scope = new ScriptScope(tokenizer, parsers, (CompositeSyntaxElement)element, newResult.FinalParser);
                        }
                        
                        break;
                    }
                }
            }
        }
    }

    public void UpdateScript()
    {
        var lineIndex = 0;
        var parsers = _bundleBuilder.GetParsers(String.Empty);
        ConvertElementToScript(RootSyntaxElement, parsers, string.Empty, ref lineIndex);
        _script.Trim(lineIndex);
    }

    private void ConvertElementToScript(SyntaxElement element, IReadOnlyCollection<IParser> parsers, string margin, ref int lineIndex)
    {
        ScriptLine? line = null;
        SynthesizeNewScopeResult? synthesizeNewScopeResult = null;
        var pair = _associations.FindScriptLines(element);
        if (pair != null)
        {
            line = pair.Value.Item1;
            synthesizeNewScopeResult = pair.Value.Item2;
        }

        IParser? parserToRegister = null;
        if (line == null)
        {
            foreach (var parser in parsers)
            {
                var synthesizeResult = parser.Synthesize(element);
                if (synthesizeResult != null)
                {
                    line = new ScriptLine(-1, margin + synthesizeResult.Value.Text);
                    synthesizeNewScopeResult = synthesizeResult.Value.NewScopeResult;
                    if (synthesizeNewScopeResult != null && !string.IsNullOrEmpty(margin))
                        synthesizeNewScopeResult = new SynthesizeNewScopeResult(synthesizeNewScopeResult.Value.Name, margin + synthesizeNewScopeResult.Value.FinalText);
                    parserToRegister = parser;
                    break;
                }
            }
        }

        if (line != null)
        {
            var newLine = _script.SetLine(line.Value, lineIndex++);
            if (parserToRegister != null)
                _associations.Register(newLine, parserToRegister, element, synthesizeNewScopeResult?.Name);
        }

        if (element is CompositeSyntaxElement composite)
        {
            var subParsers = parsers;
            if (synthesizeNewScopeResult?.Name != null)
                subParsers = _bundleBuilder.GetParsers(synthesizeNewScopeResult.Value.Name);

            var nextMargin = margin;
            if (line != null)
                nextMargin = margin + " ";
            foreach (var subElement in composite.Children)
                ConvertElementToScript(subElement, subParsers, nextMargin, ref lineIndex);
        }

        if (synthesizeNewScopeResult?.FinalText != null)
        {
            var endLine = new ScriptLine(-1, synthesizeNewScopeResult.Value.FinalText);
            var newLine = _script.SetLine(endLine, lineIndex++);
            if (parserToRegister != null)
                _associations.RegisterFinalResult(newLine, (CompositeSyntaxElement)element);
        }
    }

    private ScriptScope GetRootScriptScope()
    {
        var result = new ScriptScope(_bundleBuilder.GetTokenizer(String.Empty), _bundleBuilder.GetParsers(String.Empty), new RootSyntaxElement(), null);
        return result;
    }
}