﻿using Language.Api.Operations;
using Language.Api.Refactorings;
using Language.Api.Transfers;

namespace Language.Api;

public interface IBundle
{
    IEnumerable<(ITokenizer Tokenizer, string[] ScopeNames)> GetTokenizers();
    IEnumerable<(IParser Parser, string[] ScopeNames)> GetParsers();
    IEnumerable<IOperation> GetOperations();
    IEnumerable<ISyntaxToSemanticTransfer> GetSyntaxToSemanticTransfers();
    IEnumerable<ISyntaxRefactoring> GetSyntaxRefactorings();
}