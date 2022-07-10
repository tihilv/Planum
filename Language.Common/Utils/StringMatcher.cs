namespace Language.Common.Utils;

public static class StringMatcher
{
    public static bool Match(string str, int position, string stringToMatch)
    {
        if (position + stringToMatch.Length > str.Length)
            return false;

        if (stringToMatch.Length >= 2 && str[position + 1] != stringToMatch[1])
            return false;

        if (stringToMatch.Length >= 1 && str[position] != stringToMatch[0])
            return false;

        if (stringToMatch.Length < 3)
            return true;
        
        for (ushort index = 2; index < stringToMatch.Length; index++)
            if (str[position + index] != stringToMatch[index])
                return false;
        
        return true;
    }

}