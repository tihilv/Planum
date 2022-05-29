using Bundle.Uml.Elements;
using Language.Api.Semantic;

namespace Bundle.Uml.Semantic;

internal class UmlContextData
{
    private readonly Dictionary<string, UmlFigureSemanticElement> _figures;
    private readonly HashSet<UmlFigureSemanticElement> _elementsToDelete;

    public UmlContextData()
    {
        _figures = new Dictionary<String, UmlFigureSemanticElement>();
        _elementsToDelete = new HashSet<UmlFigureSemanticElement>();
    }

    public UmlFigureSemanticElement? Register(UmlSyntaxElement uml, UmlFigure figure)
    {
        UmlFigureSemanticElement? result = null;
        if (!_figures.TryGetValue(figure.Text, out var semanticElement))
        {
            semanticElement = new UmlFigureSemanticElement(figure.Text);
            _figures.Add(figure.Text, semanticElement);
            result = semanticElement;
        }

        semanticElement.Register(uml, figure);

        if (figure.Alias != null)
        {
            if (_figures.TryGetValue(figure.Alias, out var semanticElementForAlias))
            {
                semanticElementForAlias.RegisterAlias(figure.Text, figure.Alias);
                    
                foreach (var usage in semanticElement.Usages)
                    semanticElementForAlias.Register(usage.SyntaxElement, usage.Figure);

                _elementsToDelete.Add(semanticElement);

                _figures[figure.Text] = semanticElementForAlias;
            }
            else
            {
                semanticElement.RegisterAlias(figure.Text, figure.Alias);
                _figures.Add(figure.Alias, semanticElement);
            }
        }

        return result;
    }

    public bool IsDeleted(ISemanticElement semanticElement)
    {
        return _elementsToDelete.Contains(semanticElement);
    }

    public UmlFigureSemanticElement GetSemanticElement(string text)
    {
        return _figures[text];
    }
}