using Language.Api;
using Language.Api.Operations;
using Language.Api.Transfers;

namespace Language.Common;

public class DefaultBundleBuilder : IBundleBuilder
{
    private readonly Dictionary<string, ITokenizer> _tokenizers;
    private readonly Dictionary<string, List<IParser>> _parsers;
    private readonly Dictionary<string, List<IOperation>> _operations;
    private List<ISyntaxToSemanticTransfer> _syntaxToSemanticTransfers;

    public DefaultBundleBuilder()
    {
        _tokenizers = new Dictionary<String, ITokenizer>();
        _parsers = new Dictionary<String, List<IParser>>();
        _operations = new Dictionary<String, List<IOperation>>();
        _syntaxToSemanticTransfers = new List<ISyntaxToSemanticTransfer>();
    }

    public void RegisterBundle(IBundle bundle)
    {
        foreach (var tuple in bundle.GetTokenizers())
        {
            foreach (var scopeName in tuple.ScopeNames)
            {
                _tokenizers.Add(scopeName, tuple.Tokenizer);
            }
        }
        
        foreach (var tuple in bundle.GetParsers())
        {
            foreach (var scopeName in tuple.ScopeNames)
            {
                if (!_parsers.TryGetValue(scopeName, out var parserList))
                {
                    parserList = new List<IParser>();
                    _parsers.Add(scopeName, parserList);
                }
                
                parserList.Add(tuple.Parser);
            }
        }
        
        foreach (var operation in bundle.GetOperations())
        {
            if (!_operations.TryGetValue(operation.Definition.Id, out var list))
            {
                list = new List<IOperation>();
                _operations.Add(operation.Definition.Id, list);
            }
                
            list.Add(operation);
        }
        
        _syntaxToSemanticTransfers.AddRange(bundle.GetSyntaxToSemanticTransfers());
    }
    
    public ITokenizer GetTokenizer(String scopeName)
    {
        if (string.IsNullOrEmpty(scopeName))
            return EmptyTokenizer.Instance;
        
        return _tokenizers[scopeName];
    }

    public IReadOnlyCollection<IParser> GetParsers(String scopeName)
    {
        return _parsers[scopeName];
    }
    
    public IReadOnlyDictionary<String, List<IOperation>> GetOperations()
    {
        return _operations;
    }

    public IReadOnlyCollection<ISyntaxToSemanticTransfer> GetSyntaxToSemanticTransfers()
    {
        return _syntaxToSemanticTransfers;
    }
}