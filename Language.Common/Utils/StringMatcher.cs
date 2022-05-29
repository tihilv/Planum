namespace Language.Common.Utils;

public static class StringMatcher
{
    public static bool Match(string str, int position, string stringToMatch)
    {
        if (position + stringToMatch.Length > str.Length)
            return false;
        
        for (ushort index = 0; index < stringToMatch.Length; index++)
            if (str[position + index] != stringToMatch[index])
                return false;
        
        return true;
    }

}