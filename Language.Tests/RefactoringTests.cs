using System.Linq;
using Bundle.Uml;
using Bundle.Uml.Elements;
using Language.Api.Syntax;
using Language.Common;
using Language.Common.Refactorings;
using Language.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class RefactoringTests
{
    [TestMethod]
    public void DuplicateRefactoringTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));
        var uc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));

        rootElement.Make(fc).Last();
        rootElement.Make(uc).Last();
        rootElement.Make(fcToe1).Last();

        var refactoringManager = new DefaultRefactoringManager(builder);
        refactoringManager.Refactor(rootElement);
        
        Assert.AreEqual(1, rootElement.Children.Count);
        Assert.AreEqual(fcToe1, rootElement.Children.Single());
    }
    
    [TestMethod]
    public void SequenceRefactoringTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));
        var uc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));

        rootElement.Make(fc).Last();
        rootElement.Make(uc).Last();
        rootElement.Make(fcToe2).Last();
        rootElement.Make(fcToe1).Last();

        var refactoringManager = new DefaultRefactoringManager(builder);
        refactoringManager.Refactor(rootElement);
        
        Assert.AreEqual(2, rootElement.Children.Count);
        Assert.AreEqual(fcToe1, rootElement.Children.First());
        Assert.AreEqual(fcToe2, rootElement.Children.Last());
    }
}