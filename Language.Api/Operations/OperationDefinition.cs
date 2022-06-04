namespace Language.Api.Operations;

public struct OperationDefinition
{
    public readonly string Id;
    public readonly string Text;
    public readonly ParameterDefinition[] ParameterDefinitions;

    public OperationDefinition(String id, String text, params ParameterDefinition[] parameterDefinitions)
    {
        Id = id;
        Text = text;
        ParameterDefinitions = parameterDefinitions;
    }
}

public struct ParameterDefinition
{
    public readonly Type Type;
    public readonly string Text;
    public readonly bool Optional;

    public ParameterDefinition(Type type, string text, bool optional = false)
    {
        Type = type;
        Text = text;
        Optional = optional;
    }
}