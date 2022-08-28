using System.Linq;
using Bundle.Uml;
using Bundle.Uml.Elements;
using Bundle.Uml.Operations;
using Bundle.Uml.Semantic;
using Language.Api.Syntax;
using Language.Common;
using Language.Common.Operations;
using Language.Common.Primitives;
using Language.Common.Semantic;
using Language.Common.Transfers;
using Language.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class OperationTests
{
    [TestMethod]
    public void ChangeTextTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        model.rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcName), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "Some other element"))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var otherFcName = "Not a food critic";
        
        Assert.AreEqual(SimpleTestSyntaxModel.fcName, ChangeTextOperation.Instance.GetValue(result[0]));
        ChangeTextOperation.Instance.Execute(result, new [] {result[0]}, new [] {otherFcName});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();
        
        Assert.AreEqual(10, result.Length);

        var fcDefSem = result[0] as UmlFigureSemanticElement;
        Assert.IsNotNull(fcDefSem);
        Assert.AreEqual(otherFcName, fcDefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Alias);
        Assert.AreEqual(otherFcName, ChangeTextOperation.Instance.GetValue(fcDefSem));
    }
    
    [TestMethod]
    public void ChangeAliasTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        model.rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcName), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "Some other element"))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var otherFcAlias = "notfc";
        
        ChangeAliasOperation.Instance.Execute(result, new [] {result[0]}, new [] {otherFcAlias});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();
        
        var fcDefSem = result[0] as UmlFigureSemanticElement;
        Assert.IsNotNull(fcDefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.fcName, fcDefSem.Text);
        Assert.AreEqual(otherFcAlias, fcDefSem.Alias);
    }

    [TestMethod]
    public void SingleStatementChangeAliasTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcName), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "Some other element"))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        Assert.AreEqual(3, result.Length);
        
        ChangeAliasOperation.Instance.Execute(result, new [] {result[0]}, new [] {SimpleTestSyntaxModel.fcAlias});
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        
        Assert.AreEqual(3, result.Length);
        var fcDefSem = result[0] as UmlFigureSemanticElement;
        Assert.IsNotNull(fcDefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.fcName, fcDefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Alias);
        Assert.AreEqual(2, fcDefSem.Usages.Count);
        Assert.AreEqual(SimpleTestSyntaxModel.fcName, fcDefSem.Usages[0].Figure.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Usages[0].Figure.Alias);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Usages[1].Figure.Text);
    }
    
    [TestMethod]
    public void ChangeArrowTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var newArrow = new Arrow(ArrowShape.Circle, ArrowShape.Rectangle, 13, LineType.Dash, Direction.Right); 
        
        ChangeArrowOperation.Instance.Execute(result, new [] {result[2]}, new object[] {newArrow});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var arrowDefSem = result[2] as ArrowSemanticElement;
        Assert.IsNotNull(arrowDefSem);
        Assert.AreEqual(newArrow, arrowDefSem.Arrow);
        
        var defaultArrowDefSem = result[4] as ArrowSemanticElement;
        Assert.IsNotNull(defaultArrowDefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.defaultArrow, defaultArrowDefSem.Arrow);
    }
    
    [TestMethod]
    public void UngroupTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        Assert.AreEqual(1, result.Count(r=>r is IGroupSemantic));
        
        UngroupOperation.Instance.Execute(result, new [] {result[7]}, new object[] {});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        Assert.AreEqual(0, result.Count(r=>r is IGroupSemantic));
        Assert.AreEqual(7, result.Length);
        
        var syntaxChildren = model.rootElement.Children.ToArray();
        Assert.AreEqual(7, syntaxChildren.Length);
        Assert.AreEqual(model.uc1Def, syntaxChildren[4]);
        Assert.AreEqual(model.uc2Def, syntaxChildren[5]);
        Assert.AreEqual(model.uc3Def, syntaxChildren[6]);
    }
    
    [TestMethod]
    public void CreateUmlRectangleTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var someNewElement1Name = "SE1";
        var someNewElement2Name = "SE2";
        
        var model = new SimpleTestSyntaxModel();
        model.rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, someNewElement1Name), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, someNewElement2Name))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        CreateUmlContainerOperation.Instance.Execute(result, new [] {result[0]}, new object[] {});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        Assert.AreEqual(2, result.Count(r=>r is IGroupSemantic));
        Assert.AreEqual(12, result.Length);

        CreateUmlContainerOperation.Instance.Execute(result, new [] {result[9]}, new object[] {});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        Assert.AreEqual(3, result.Count(r=>r is IGroupSemantic));
        Assert.AreEqual(13, result.Length);

        var lastContainer = result[9] as UmlContainerSemanticElement;
        Assert.IsNotNull(lastContainer);
        Assert.AreEqual(1, lastContainer.GroupSyntaxElement.Children.Count);
        var singleContainerizedElement = lastContainer.GroupSyntaxElement.Children.Single() as UmlSyntaxElement;
        Assert.IsNotNull(singleContainerizedElement);
        Assert.AreEqual(someNewElement1Name, singleContainerizedElement.FirstFigure.Text);
    }
    
    [TestMethod]
    public void AddToGroupTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        model.rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcName), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "Some other element"))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var container = result[7] as UmlContainerSemanticElement;
        var fcElement = result[0];
        AddToGroupOperation.Instance.Execute(result, new [] {container, fcElement}, null);
        Assert.AreEqual(5, model.rootElement.Children.Count);
        Assert.IsTrue((((UmlContainerSyntaxElement)model.rootElement.Children.ToArray()[3])).Children.ToArray().Contains(model.fcDef));
    }
    
    [TestMethod]
    public void ExcludeFromGroupTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();

        var elementToExclude = result[3];
        ExcludeFromGroupOperation.Instance.Execute(result, new [] {elementToExclude}, null);
        Assert.AreEqual(6, model.rootElement.Children.Count);
        Assert.AreEqual(model.uc2Def, model.rootElement.Children.ToArray()[4]);
        
        // second extraction shouldn't affect the model
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();
        elementToExclude = result[3];
        ExcludeFromGroupOperation.Instance.Execute(result, new [] {elementToExclude}, null);
        Assert.AreEqual(6, model.rootElement.Children.Count);
        Assert.AreEqual(model.uc2Def, model.rootElement.Children.ToArray()[4]);
    }

    [TestMethod]
    public void SimpleMoveNextTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var fcToe3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc3Alias));

        rootElement.Make(fcToe1).Last();
        rootElement.Make(fcToe2).Last();
        rootElement.Make(fcToe3).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        var elementToMove = result[1];
        MoveNextOperation.Instance.Execute(result, new [] {elementToMove}, null);
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        Assert.AreEqual(7, result.Length);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, ((ITextedSemantic)result[0]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, ((ITextedSemantic)result[1]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, ((ITextedSemantic)result[2]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, ((ITextedSemantic)result[5]).Text);
    }

    
    [TestMethod]
    public void CombinedMoveNextTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var e1toe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var e3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc3Alias));

        rootElement.Make(e1toe2).Last();
        rootElement.Make(e3).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        var elementToMove = result[1];
        MoveNextOperation.Instance.Execute(result, new [] {elementToMove}, null);
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        Assert.AreEqual(4, result.Length);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, ((ITextedSemantic)result[0]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, ((ITextedSemantic)result[1]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, ((ITextedSemantic)result[2]).Text);
    }
    
    [TestMethod]
    public void SimpleMovePrevTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var fcToe3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc3Alias));

        rootElement.Make(fcToe1).Last();
        rootElement.Make(fcToe2).Last();
        rootElement.Make(fcToe3).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        var elementToMove = result[1];
        MovePrevOperation.Instance.Execute(result, new [] {elementToMove}, null);
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        Assert.AreEqual(7, result.Length);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, ((ITextedSemantic)result[0]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, ((ITextedSemantic)result[1]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, ((ITextedSemantic)result[3]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, ((ITextedSemantic)result[5]).Text);
    }
    
    [TestMethod]
    public void SimpleSecondMovePrevTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var fcToe3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc3Alias));

        rootElement.Make(fcToe1).Last();
        rootElement.Make(fcToe2).Last();
        rootElement.Make(fcToe3).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        var elementToMove = result[3];
        MovePrevOperation.Instance.Execute(result, new [] {elementToMove}, null);
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        Assert.AreEqual(7, result.Length);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, ((ITextedSemantic)result[0]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, ((ITextedSemantic)result[1]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, ((ITextedSemantic)result[2]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, ((ITextedSemantic)result[5]).Text);
    }

    
    [TestMethod]
    public void CombinedMovePrevTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var e1toe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var e3 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc3Alias));

        rootElement.Make(e1toe2).Last();
        rootElement.Make(e3).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        var elementToMove = result[3];
        MovePrevOperation.Instance.Execute(result, new [] {elementToMove}, null);
        result = semanticConverter.GetSemanticElements(rootElement).ToArray();
        Assert.AreEqual(4, result.Length);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, ((ITextedSemantic)result[0]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, ((ITextedSemantic)result[1]).Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, ((ITextedSemantic)result[2]).Text);
    }
}