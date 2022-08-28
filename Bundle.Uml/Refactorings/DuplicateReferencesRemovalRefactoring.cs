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
        var elementPositions = new Dictionary<String, int>();
        var children = compositeSyntaxElement.Children.ToArray();
        for (int index = 0; index < children.Length; index++)
        {
            var syntaxElement = children[index] as UmlSyntaxElement;
            if (syntaxElement != null)
            {
                RegisterFigure(elementPositions, syntaxElement.FirstFigure, index);
                if (syntaxElement.SecondFigure != null)
                    RegisterFigure(elementPositions, syntaxElement.SecondFigure.Value, index);
            }
        }
        
        UmlSyntaxElement? lastElement = null;
        for (int index = children.Length - 1; index >= 0; index--)
        {
            var syntaxElement = children[index];
            var umlSyntaxElement = syntaxElement as UmlSyntaxElement;
            if (umlSyntaxElement != null && lastElement != null)
            {
                if (umlSyntaxElement.IsSingleDefinition && umlSyntaxElement.FirstFigure.IsSingleFigure)
                {
                    if (lastElement.FirstFigure.Text == umlSyntaxElement.FirstFigure.Text || lastElement.SecondFigure?.Text == umlSyntaxElement.FirstFigure.Text)
                    {
                        var firstIndex = FindFigureIndex(elementPositions, lastElement.FirstFigure)??index;
                        var secondIndex = ((lastElement.SecondFigure != null) ? FindFigureIndex(elementPositions, lastElement.SecondFigure.Value) : null)??index;
                        if (firstIndex <= secondIndex)
                        {
                            compositeSyntaxElement.Make(umlSyntaxElement).Deleted();
                            umlSyntaxElement = lastElement;
                        }
                    }
                }
            }

            lastElement = umlSyntaxElement;
        }
    }

    private void RegisterFigure(Dictionary<String, Int32> elementPositions, UmlFigure figure, Int32 index)
    {
        if (!string.IsNullOrEmpty(figure.Text))
            elementPositions.TryAdd(figure.Text, index);
        if (!string.IsNullOrEmpty(figure.Alias))
            elementPositions.TryAdd(figure.Alias, index);
    }
    
    private int? FindFigureIndex(Dictionary<String, int> elementPositions, UmlFigure figure)
    {
        if (elementPositions.TryGetValue(figure.Text, out var element))
            return element;

        if (!string.IsNullOrEmpty(figure.Alias) && elementPositions.TryGetValue(figure.Alias, out element))
            return element;

        return null;
    }
}