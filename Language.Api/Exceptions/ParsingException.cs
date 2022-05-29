namespace Language.Api.Exceptions;

public class ParsingException : Exception
{
    public CharacterRange Range { get; }

    public ParsingException(CharacterRange range, string message): base(message)
    {
        Range = range;
    }
}