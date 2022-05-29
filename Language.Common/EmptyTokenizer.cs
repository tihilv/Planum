using Language.Api;

namespace Language.Common;

public class EmptyTokenizer : ITokenizer
{
    public static readonly ITokenizer Instance = new EmptyTokenizer();
    private EmptyTokenizer()
    {
        
    }
    public IEnumerable<Token> GetTokens(ScriptLine line)
    {
        yield return new Token(line, new CharacterRange(0, (ushort)(line.Value.Length - 1)));
    }
}