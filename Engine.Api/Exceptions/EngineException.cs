namespace Engine.Api.Exceptions;

public class EngineException : Exception
{
    public int LineNumber { get; }

    public EngineException(String? message, int lineNumber) : base(message)
    {
        LineNumber = lineNumber;
    }
}