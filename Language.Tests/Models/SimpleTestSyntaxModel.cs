using Bundle.Uml.Elements;
using Language.Api.Syntax;
using Language.Common.Primitives;

namespace Language.Tests.Models;

internal class SimpleTestSyntaxModel
{
    public const string fcName = "Food Critic";
    public const string fcAlias = "fc";

    public const string uc1Name = "Eat Food";
    public const string uc1Alias = "UC1";
    public const string uc2Name = "Pay for Food";
    public const string uc2Alias = "UC2";
    public const string uc3Name = "Drink";
    public const string uc3Alias = "UC3";

    public readonly RootSyntaxElement rootElement;
    
    public readonly UmlSyntaxElement fcToUc1;
    public readonly UmlSyntaxElement fcToUc2;
    public readonly UmlSyntaxElement fcToUc3;

    public readonly UmlSyntaxElement fcDef;

    public readonly UmlSyntaxElement uc1Def;
    public readonly UmlSyntaxElement uc2Def;
    public readonly UmlSyntaxElement uc3Def;

    public SimpleTestSyntaxModel()
    {
        fcToUc1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, fcAlias), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, uc1Alias));
        fcToUc2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, fcAlias), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, uc2Alias));
        fcToUc3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, fcAlias), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, uc3Alias));

        fcDef = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, fcName, null, fcAlias));

        uc1Def = new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, uc1Name, null, uc1Alias));
        uc2Def = new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, uc2Name, null, uc2Alias));
        uc3Def = new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, uc3Name, null, uc3Alias));

        rootElement = new RootSyntaxElement();
        rootElement.Make(fcToUc1).Last();
        rootElement.Make(fcToUc2).Last();
        rootElement.Make(fcToUc3).Last();
        rootElement.Make(fcDef).Last();

        var rectElement = new UmlContainerSyntaxElement(UmlContainerType.Rectangle, "Restaurant");
        rootElement.Make(rectElement).Last();
        
        rectElement.Make(uc1Def).Last();
        rectElement.Make(uc2Def).Last();
        rectElement.Make(uc3Def).Last();
    }
}