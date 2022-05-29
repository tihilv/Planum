namespace Language.Api;

public struct ScriptLine
{
    public readonly int Identifier;
    public readonly string Value;

    public ScriptLine(int identifier, string value)
    {
        Identifier = identifier;
        Value = value;
    }
}