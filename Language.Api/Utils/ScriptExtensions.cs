namespace Language.Api.Utils;

public static class ScriptExtensions
{
    public static string GetAllText(this IScript script)
    {
        return String.Join(Environment.NewLine, script.GetLines().Select(l => l.Value));
    }
}