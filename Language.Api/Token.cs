namespace Language.Api;

public class Token
{
    public ScriptLine ScriptLine { get; }
    public CharacterRange Range { get; }
    public string Value { get; }

    public Token(ScriptLine scriptLine, CharacterRange range)
    {
        ScriptLine = scriptLine;
        Range = range;
        Value = scriptLine.Value.Substring(range.FromChar, range.ToChar - range.FromChar + 1);
    }

    public override String ToString()
    {
        return $"{Range} {Value}";
    }
}