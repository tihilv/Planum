using Language.Api.Operations;
using Language.Api.Transfers;

namespace Language.Api;

public interface IBundleBuilder
{
    void RegisterBundle(IBundle bundle);
    public ITokenizer GetTokenizer(string scopeName);
    IReadOnlyCollection<IParser> GetParsers(string scopeName);
    IReadOnlyDictionary<String, List<IOperation>> GetOperations();
    IReadOnlyCollection<ISyntaxToSemanticTransfer> GetSyntaxToSemanticTransfers();
}