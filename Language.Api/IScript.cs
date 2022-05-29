namespace Language.Api;

public interface IScript
{
    IEnumerable<ScriptLine> GetLines();
}