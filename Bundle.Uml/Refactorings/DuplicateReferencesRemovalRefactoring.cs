using Bundle.Uml.Elements;
using Language.Api.Refactorings;
using Language.Api.Syntax;

namespace Bundle.Uml.Refactorings;

public class DuplicateReferencesRemovalRefactoring: ISyntaxRefactoring
{
    public static readonly ISyntaxRefactoring Instance = new DuplicateReferencesRemovalRefactoring();
    
    private DuplicateReferencesRemovalRefactoring()
    {
        
    }
    
    public Byte Priority => 101;
    
    public void Refactor(ICompositeSyntaxElement compositeSyntaxElement)
    {
        Dictionary<string, UmlSyntaxElementWithPosition> elements = new Dictionary<String, UmlSyntaxElementWithPosition>();
        UmlSyntaxElement? lastElement = null;
        foreach (var syntaxElement in compositeSyntaxElement.Children.Reverse().ToArray())
        {
            var umlSyntaxElement = syntaxElement as UmlSyntaxElement;
            if (umlSyntaxElement != null && lastElement != null)
            {
                if (umlSyntaxElement.IsSingleDefinition && umlSyntaxElement.FirstFigure.IsSingleFigure)
                {
                    if (lastElement.FirstFigure.Text == umlSyntaxElement.FirstFigure.Text || lastElement.SecondFigure?.Text == umlSyntaxElement.FirstFigure.Text)
                    {
                        compositeSyntaxElement.Make(umlSyntaxElement).Deleted();
                        umlSyntaxElement = lastElement;
                    }
                }
            }

            lastElement = umlSyntaxElement;
        }
    }

    private UmlSyntaxElementWithPosition? FindExistingElement(Dictionary<String, UmlSyntaxElementWithPosition> elements, UmlFigure figure)
    {
        if (elements.TryGetValue(figure.Text, out var element))
            return element;

        if (!string.IsNullOrEmpty(figure.Alias) && elements.TryGetValue(figure.Alias, out element))
            return element;

        return null;
    }
    
    private class UmlSyntaxElementWithPosition
    {
        public readonly UmlSyntaxElement Element;
        public readonly int Index;

        public UmlSyntaxElementWithPosition(UmlSyntaxElement element, Int32 index)
        {
            Element = element;
            Index = index;
        }
    }
}