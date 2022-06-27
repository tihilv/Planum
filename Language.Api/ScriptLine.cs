namespace Language.Api;

public struct ScriptLine
{
    public readonly int Identifier;
    public readonly string Value;

    public ScriptLine(string value): this(-1, value)
    {
    }
    
    public ScriptLine(int identifier, string value)
    {
        Identifier = identifier;
        Value = value;
    }

    public override String ToString()
    {
        return $"{Identifier}: {Value}";
    }
}