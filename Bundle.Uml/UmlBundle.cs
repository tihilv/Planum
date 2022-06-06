using Bundle.Uml.Operations;
using Bundle.Uml.Parsers;
using Bundle.Uml.Tokenizer;
using Bundle.Uml.Transfers;
using Language.Api;
using Language.Api.Operations;
using Language.Api.Transfers;
using Language.Common.Operations;
using Language.Common.Parsers;

namespace Bundle.Uml;

public class UmlBundle: IBundle
{
    public static readonly IBundle Instance = new UmlBundle();
    
    private UmlBundle()
    { }
    
    public const string Name = "uml";
    
    public IEnumerable<(ITokenizer Tokenizer, String[] ScopeNames)> GetTokenizers()
    {
        yield return new(UmlTokenizer.Instance, new[] { Name });
    }

    public IEnumerable<(IParser Parser, String[] ScopeNames)> GetParsers()
    {
        yield return new(StartUmlRootParser.Instance, new[] { string.Empty });
        
        yield return new(IncludeUrlParser.Instance, new[] { Name });
        yield return new(IncludeParser.Instance, new[] { Name });
        yield return new(CommentsParser.Instance, new[] { Name });
        yield return new(DirectionParser.Instance, new[] { Name });
        
        yield return new(UmlContainerElementParser.Instance, new[] { Name });
        yield return new(UmlElementParser.Instance, new[] { Name });
    }

    public IEnumerable<IOperation> GetOperations()
    {
        yield return ChangeTextOperation.Instance;
        yield return ChangeAliasOperation.Instance;
        yield return ChangeArrowOperation.Instance;
        yield return UngroupOperation.Instance;
        yield return AddToGroupOperation.Instance;
        yield return ExcludeFromGroupOperation.Instance;
        yield return CreateUmlContainerOperation.Instance;
        yield return MoveNextOperation.Instance;
        yield return MovePrevOperation.Instance;
    }

    public IEnumerable<ISyntaxToSemanticTransfer> GetSyntaxToSemanticTransfers()
    {
        yield return UmlContainerTransfer.Instance;
        yield return UmlFigureTransfer.Instance;
        yield return UmlArrowTransfer.Instance;
    }
}