using Language.Api.Operations;

namespace Language.Common.Operations;

public static class DefaultOperationDefinitions
{
    public static readonly OperationDefinition ChangeText = new OperationDefinition("TEXT.CHANGE", "Text", new ParameterDefinition(typeof(string), "New text"));
    public static readonly OperationDefinition ChangeAlias = new OperationDefinition("ALIAS.CHANGE", "Id", new ParameterDefinition(typeof(string), "New id"));
}