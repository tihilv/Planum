using System.Linq;
using Bundle.Uml;
using Bundle.Uml.Elements;
using Bundle.Uml.Semantic;
using Language.Api.Syntax;
using Language.Common;
using Language.Common.Primitives;
using Language.Common.Transfers;
using Language.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class SyntaxToSemanticTests
{
    [TestMethod]
    public void TextAndAliasProcessingTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcName, alias: SimpleTestSyntaxModel.fcAlias))).Last();
        rootElement.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "Some other element"))).Last();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(rootElement).ToArray();

        Assert.AreEqual(3, result.Length);

    }

    [TestMethod]
    public void SimpleModelProcessingTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var model = new SimpleTestSyntaxModel();
        
        var semanticConverter = new DefaultSemanticConverter(builder);
        var result = semanticConverter.GetSemanticElements(model.rootElement).ToArray();
        
        Assert.AreEqual(8, result.Length);
        var fcDefSem = result[0] as UmlFigureSemanticElement;
        Assert.IsNotNull(fcDefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.fcName, fcDefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.fcAlias, fcDefSem.Alias);

        var uc1DefSem = result[1] as UmlFigureSemanticElement;
        Assert.IsNotNull(uc1DefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Name, uc1DefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc1Alias, uc1DefSem.Alias);
        
        var fcToUc1Sem = result[2] as ArrowSemanticElement;
        Assert.IsNotNull(fcToUc1Sem);
        Assert.AreEqual(fcDefSem, fcToUc1Sem.FirstSemanticElement);
        Assert.AreEqual(uc1DefSem, fcToUc1Sem.SecondSemanticElement);

        var uc2DefSem = result[3] as UmlFigureSemanticElement;
        Assert.IsNotNull(uc2DefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Name, uc2DefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc2Alias, uc2DefSem.Alias);

        var fcToUc2Sem = result[4] as ArrowSemanticElement;
        Assert.IsNotNull(fcToUc2Sem);
        Assert.AreEqual(fcDefSem, fcToUc2Sem.FirstSemanticElement);
        Assert.AreEqual(uc2DefSem, fcToUc2Sem.SecondSemanticElement);

        var uc3DefSem = result[5] as UmlFigureSemanticElement;
        Assert.IsNotNull(uc3DefSem);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Name, uc3DefSem.Text);
        Assert.AreEqual(SimpleTestSyntaxModel.uc3Alias, uc3DefSem.Alias);

        var fcToUc3Sem = result[6] as ArrowSemanticElement;
        Assert.IsNotNull(fcToUc3Sem);
        Assert.AreEqual(fcDefSem, fcToUc3Sem.FirstSemanticElement);
        Assert.AreEqual(uc3DefSem, fcToUc3Sem.SecondSemanticElement);

        var containerSem = result[7] as UmlContainerSemanticElement;
        Assert.IsNotNull(containerSem);
        Assert.AreEqual(SimpleTestSyntaxModel.containerName, containerSem.Id);
        Assert.AreEqual(3, containerSem.GroupSyntaxElement.Children.Count);
    }
}