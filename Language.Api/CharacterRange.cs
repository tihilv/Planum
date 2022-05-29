namespace Language.Api;

public struct CharacterRange
{
    public readonly ushort FromChar;
    public readonly ushort ToChar;

    public CharacterRange(UInt16 fromChar, UInt16 toChar)
    {
        if (toChar < fromChar)
            throw new ArgumentException("End of the range should be later than its start");
        
        FromChar = fromChar;
        ToChar = toChar;
    }

    public override String ToString()
    {
        return $"[{FromChar} - {ToChar}]";
    }
}