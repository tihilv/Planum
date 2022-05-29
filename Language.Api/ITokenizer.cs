namespace Language.Api;

public interface ITokenizer
{
    IEnumerable<Token> GetTokens(ScriptLine line);
}