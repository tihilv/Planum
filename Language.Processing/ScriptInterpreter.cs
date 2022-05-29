using Language.Api;
using Language.Api.Exceptions;
using Language.Api.Syntax;

namespace Language.Processing;

public class ScriptInterpreter
{
    private readonly IScript _script;
    private readonly IBundleBuilder _bundleBuilder;

    private readonly ScriptAssociations _associations;
    
    public CompositeSyntaxElement RootSyntaxElement { get; private set; }

    public ScriptInterpreter(IScript script, IBundleBuilder bundleBuilder)
    {
        _script = script;
        _bundleBuilder = bundleBuilder;

        _associations = new ScriptAssociations();
    }

    public void Process()
    {
        var scopeStack = new Stack<ScriptScope>();
        var scope = GetRootScriptScope();
        RootSyntaxElement = scope.CurrentSyntaxElement;
        
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
                        _associations.Register(line, parser, element);

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

    private ScriptScope GetRootScriptScope()
    {
        var result = new ScriptScope(_bundleBuilder.GetTokenizer(String.Empty), _bundleBuilder.GetParsers(String.Empty), new RootSyntaxElement(), null);
        return result;
    }
}