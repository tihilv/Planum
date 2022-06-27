namespace Language.Api;

public interface IScript
{
    IEnumerable<ScriptLine> GetLines();
    ScriptLine SetLine(ScriptLine line, int index);
    void Trim(int lineCount);
}