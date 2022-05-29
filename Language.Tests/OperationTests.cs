using System.Linq;
using Bundle.Uml;
using Bundle.Uml.Elements;
using Bundle.Uml.Semantic;
using Language.Api.Syntax;
using Language.Common;
using Language.Common.Operations;
using Language.Common.Primitives;
using Language.Common.Semantic;
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
        
        ChangeTextOperation.Instance.Execute(result, new [] {result[0]}, new [] {otherFcName});
        result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();
        
        Assert.AreEqual(9, result.Length);

        var fcDefSem = result[0] as UmlFigureSemanticElement;
        Assert.IsNotNull(fcDefSem);
        Assert.AreEqual(otherFcName, fcDefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Alias);
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
}