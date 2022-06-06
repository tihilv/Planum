using Bundle.Uml.Elements;
using Language.Api.Refactorings;
using Language.Api.Syntax;

namespace Bundle.Uml.Refactorings;

public class SequenceRefactoring: ISyntaxRefactoring
{
    public static readonly ISyntaxRefactoring Instance = new SequenceRefactoring();
    
    private SequenceRefactoring()
    {
        
    }
    
    public Byte Priority => 100;
    
    public void Refactor(ICompositeSyntaxElement compositeSyntaxElement)
    {
        Dictionary<string, UmlSyntaxElementWithPosition> elements = new Dictionary<String, UmlSyntaxElementWithPosition>();
        int index = 0;
        foreach (var syntaxElement in compositeSyntaxElement.Children.ToArray())
        {
            index++;
            if (syntaxElement is UmlSyntaxElement umlSyntaxElement)
            {
                if (umlSyntaxElement.IsSingleDefinition)
                {
                    var existingElement = FindExistingElement(elements, umlSyntaxElement.FirstFigure);
                    if (existingElement != null)
                    {
                        if (existingElement.Element.FirstFigure.Equals(umlSyntaxElement.FirstFigure))
                        {
                            compositeSyntaxElement.Make(umlSyntaxElement).Deleted();
                            break;
                        }
                    }

                    elements.Add(umlSyntaxElement.FirstFigure.Text, new UmlSyntaxElementWithPosition(umlSyntaxElement, index));
                    if (!string.IsNullOrEmpty(umlSyntaxElement.FirstFigure.Alias))
                        elements.Add(umlSyntaxElement.FirstFigure.Alias, new UmlSyntaxElementWithPosition(umlSyntaxElement, index));
                }
                else if (umlSyntaxElement.SecondFigure != null)
                {
                    var firstExistingElement = FindExistingElement(elements, umlSyntaxElement.FirstFigure);
                    var secondExistingElement = FindExistingElement(elements, umlSyntaxElement.SecondFigure.Value);
                    if (firstExistingElement != null && secondExistingElement != null)
                    {
                        var lastElement = (firstExistingElement.Index > secondExistingElement.Index) ? firstExistingElement : secondExistingElement;
                        compositeSyntaxElement.Make(umlSyntaxElement).After(lastElement.Element);
                    }
                }
            }
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