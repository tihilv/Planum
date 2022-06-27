using Language.Api;
using Language.Common.Utils;

namespace Bundle.Uml.Tokenizer;

public class UmlTokenizer : ITokenizer
{
    public static readonly ITokenizer Instance = new UmlTokenizer();

    private readonly Braces[] _braces;
    private readonly Separator[] _separators;
    
    private UmlTokenizer()
    {
        _braces = new[]
        {
            new Braces("<<", ">>"),
            new Braces("[[", "]]"),
            new Braces(":", ":"),
            new Braces("(", ")"),
            new Braces("[", "]"),
            new Braces("\"", "\""),
        };

        _separators = new[]
        {
            new Separator(" ")
        };
    }

    public IEnumerable<Token> GetTokens(ScriptLine line)
    {
        Braces? inBraces = null;

        ushort startPosition = 0;
        
        for(ushort index =0; index<line.Value.Length;index++)
        {
            if (inBraces != null)
            {
                if (inBraces.Value.IsClose(line, index))
                {
                    index += (ushort)(inBraces.Value.Close.Length - 1);
                    yield return new Token(line, new CharacterRange(startPosition, index));
                    startPosition = (ushort)(index + 1);
                    inBraces = null;
                }
            }
            else
            {
                var token = ProcessSeparator(line, ref index, ref startPosition);
                if (token != null)
                    yield return token;
                else
                {
                    token = ProcessOpenBracket(line, ref index, ref startPosition, ref inBraces);
                    if (token != null)
                        yield return token;
                }
            }
        }

        if (inBraces != null)
        {
            var endBraces = (ushort)(startPosition + inBraces.Value.Open.Length - 1);
            yield return new Token(line, new CharacterRange(startPosition, endBraces));
            startPosition = (ushort)(endBraces + 1);
        }
        
        if (startPosition < line.Value.Length)
            yield return new Token(line, new CharacterRange(startPosition, (ushort)(line.Value.Length-1)));
    }

    private Token? ProcessSeparator(ScriptLine line, ref ushort index, ref ushort startPosition)
    {
        Token? result = null;
        foreach (var separator in _separators)
        {
            if (separator.IsPresent(line, index))
            {
                if (startPosition < index - 1)
                    result = new Token(line, new CharacterRange(startPosition, (ushort)(index - 1)));

                index += (ushort)(separator.Value.Length - 1);
                startPosition = (ushort)(index + 1);
                break;
            }
        }

        return result;
    }
    
    private Token? ProcessOpenBracket(ScriptLine line, ref ushort index, ref ushort startPosition, ref Braces? inBraces)
    {
        Token? result = null;
        foreach (var bracket in _braces)
        {
            if (bracket.IsOpen(line, index))
            {
                if (startPosition < index - 1)
                    result = new Token(line, new CharacterRange(startPosition, (ushort)(index - 1)));

                startPosition = (index);
                index += (ushort)(bracket.Open.Length - 1);
                inBraces = bracket;
                break;
            }
        }

        return result;
    }
}

internal struct Braces
{
    public readonly string Open;
    public readonly string Close;

    public Braces(String open, String close)
    {
        Open = open;
        Close = close;
    }

    public bool IsOpen(ScriptLine line, ushort position) => StringMatcher.Match(line.Value, position, Open);
    
    public bool IsClose(ScriptLine line, ushort position) => StringMatcher.Match(line.Value, position, Close);
}

internal struct Separator
{
    public readonly string Value;

    public Separator(String value)
    {
        Value = value;
    }

    public bool IsPresent(ScriptLine line, ushort position)
    {
        return StringMatcher.Match(line.Value, position, Value);
    }
}