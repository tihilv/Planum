using Language.Api;

namespace Language.Common;

public class TextScript : IScript
{
    private readonly Dictionary<int, ScriptLine> _lines;
    private readonly List<int> _lineNumbers;

    private int _nextIndex;

    private TextScript()
    {
        _nextIndex = 0;
        _lines = new Dictionary<Int32, ScriptLine>();
        _lineNumbers = new List<Int32>();
    }

    public TextScript(IEnumerable<string> lines) : this()
    {
        var lengthKnown = lines.TryGetNonEnumeratedCount(out var desiredLength);
        if (lengthKnown)
            EnsureLength(desiredLength - 1);

        int index = 0;
        foreach (var line in lines)
            SetLine(new ScriptLine(line), index++);
    }

    public IEnumerable<ScriptLine> GetLines()
    {
        foreach (var number in _lineNumbers)
            if (number >= 0)
                yield return _lines[number];
    }

    public ScriptLine SetLine(ScriptLine line, int index)
    {
        if (line.Identifier < 0)
            line = new ScriptLine(_nextIndex++, line.Value);

        EnsureLength(index);

        _lines[line.Identifier] = line;
        _lineNumbers[index] = line.Identifier;

        return line;
    }

    public void Trim(Int32 lineCount)
    {
        for (int i = _lineNumbers.Count-1; i >= lineCount; i--)
        {
            _lines.Remove(_lineNumbers[i]);
            _lineNumbers.RemoveAt(i);
        }
    }

    private void EnsureLength(int index)
    {
        if (_lineNumbers.Count <= index)
            _lineNumbers.AddRange(Enumerable.Repeat(-1, index - _lineNumbers.Count+1));
    }
}