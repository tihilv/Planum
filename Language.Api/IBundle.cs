using Language.Api.Operations;
using Language.Api.Semantic;

namespace Language.Api;

public interface IBundle
{
    IEnumerable<(ITokenizer Tokenizer, string[] ScopeNames)> GetTokenizers();
    IEnumerable<(IParser Parser, string[] ScopeNames)> GetParsers();
    IEnumerable<IOperation> GetOperations();
    IEnumerable<ISyntaxToSemanticTransfer> GetSyntaxToSemanticTransfers();
}