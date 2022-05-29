namespace Language.Api;

public interface IParser
{
    ParseResult? Parse(Token[] tokens);
}