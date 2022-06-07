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
        var elements = new Dictionary<String, UmlSyntaxElementWithPosition>();
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
                        if (existingElement.Element.FirstFigure.Equals(umlSyntaxElement.FirstFigure) || existingElement.Element.SecondFigure?.Equals(umlSyntaxElement.FirstFigure) == true)
                        {
                            compositeSyntaxElement.Make(umlSyntaxElement).Deleted();
                            break;
                        }
                    }

                    RegisterFigure(elements, umlSyntaxElement, umlSyntaxElement.FirstFigure, index);
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
                    
                    RegisterFigure(elements, umlSyntaxElement, umlSyntaxElement.FirstFigure, index);
                    RegisterFigure(elements, umlSyntaxElement, umlSyntaxElement.SecondFigure.Value, index);
                }
            }
        }
    }
    
    private void RegisterFigure(Dictionary<String, UmlSyntaxElementWithPosition> elements, UmlSyntaxElement umlSyntaxElement, UmlFigure figure, int index)
    {
        elements.TryAdd(figure.Text, new UmlSyntaxElementWithPosition(umlSyntaxElement, index));
        if (!string.IsNullOrEmpty(figure.Alias))
            elements.TryAdd(figure.Alias, new UmlSyntaxElementWithPosition(umlSyntaxElement, index));
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