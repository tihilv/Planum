using Bundle.Uml.Elements;
using Language.Api.Semantic;
using Language.Common.Operations;

namespace Bundle.Uml.Semantic;

public class UmlFigureSemanticElement: ISemanticElement, ITexted, IAliased
{
    private readonly List<Usage> _usages;
    private string _text;
    private string? _alias;

    public List<Usage> Usages => _usages;

    public String Text
    {
        get { return _text; }
        set
        {
            SetText(value);
        }
    }

    public String? Alias
    {
        get { return _alias; }
        set { SetAlias(value); }
    }

    public String Id => _alias ?? _text;
    
    
    public UmlFigureSemanticElement(String name)
    {
        _text = name;

        _usages = new List<Usage>();
    }


    internal void Register(UmlSyntaxElement uml, UmlFigure figure)
    {
        _usages.Add(new Usage(uml, figure));
    }

    internal void RegisterAlias(string name, string alias)
    {
        _text = name;
        _alias = alias;
    }
    
    private void SetText(String newText)
    {
        foreach (var usage in Usages)
        {
            if (usage.Figure.Text.Equals(Text))
                ReplaceFigure(usage, usage.Figure.With(text: newText));
        }

        _text = newText;
    }

    private void SetAlias(String newAlias)
    {
        if (string.IsNullOrEmpty(Alias))
        {
            // search for an appropriate usage
            var appropriateUsage = Usages.FirstOrDefault(u => u.SyntaxElement.Arrow == null && u.SyntaxElement.SecondFigure == null);
            if (appropriateUsage != null)
            {
                ReplaceFigure(appropriateUsage, appropriateUsage.Figure.With(alias: newAlias));
            }
            else
            {
                // create new Alias record
                var usageToTake = Usages.MaxBy(u => u.Figure.Type);
                var figure = usageToTake!.Figure.With(alias: newAlias);
                UmlSyntaxElement newSyntaxElement = new UmlSyntaxElement(figure);
                usageToTake.SyntaxElement.Parent!.Make(newSyntaxElement).Before(Usages.First().SyntaxElement);
                Usages.Insert(0, new Usage(newSyntaxElement, figure));
            }

            foreach (var usage in Usages)
            {
                if (usage.Figure.Alias == null && usage.Figure.Text.Equals(Text))
                    ReplaceFigure(usage, usage.Figure.With(text: newAlias));
            }
        }
        else
        {
            // replace all aliases
            foreach (var usage in Usages)
            {
                if (usage.Figure.Alias?.Equals(Alias) == true)
                    ReplaceFigure(usage, usage.Figure.With(alias: newAlias));
                
                // ... and texts that equals to alias
                else if (usage.Figure.Text.Equals(Alias))
                    ReplaceFigure(usage, usage.Figure.With(text: newAlias));
            }
        }

        _alias = newAlias;
    }

    private void ReplaceFigure(Usage usage, UmlFigure newFigure)
    {
        UmlSyntaxElement newSyntaxElement = (usage.SyntaxElement.FirstFigure.Equals(usage.Figure)) ? usage.SyntaxElement.With(firstFigure: newFigure) : usage.SyntaxElement.With(secondFigure: newFigure);
        usage.SyntaxElement.Parent!.Make(newSyntaxElement).Replacing(usage.SyntaxElement);
        usage.SyntaxElement = newSyntaxElement;
        usage.Figure = newFigure;
    }

    public class Usage
    {
        public UmlSyntaxElement SyntaxElement;
        public UmlFigure Figure;

        public Usage(UmlSyntaxElement syntaxElement, UmlFigure figure)
        {
            SyntaxElement = syntaxElement;
            Figure = figure;
        }
    }
}