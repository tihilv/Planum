namespace Language.Api.Exceptions;

public class LineException : Exception
{
    public ScriptLine Line { get; }

    public LineException(ScriptLine line, String? message) : base(message)
    {
        Line = line;
    }
}