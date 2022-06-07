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
    public void DuplicateTest()
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
    public void SingleDuplicateTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var fc1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fc2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));

        rootElement.Make(fc1).Last();
        rootElement.Make(fc2).Last();
        rootElement.Make(fcToe1).Last();

        var refactoringManager = new DefaultRefactoringManager(builder);
        refactoringManager.Refactor(rootElement);
        
        Assert.AreEqual(1, rootElement.Children.Count);
        Assert.AreEqual(fcToe1, rootElement.Children.Single());
    }
    
    [TestMethod]
    public void SingleOrderedDuplicateTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var uc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));

        rootElement.Make(uc).Last();
        rootElement.Make(fcToe1).Last();
        rootElement.Make(fc).Last();

        var refactoringManager = new DefaultRefactoringManager(builder);
        refactoringManager.Refactor(rootElement);
        
        Assert.AreEqual(2, rootElement.Children.Count);
        Assert.AreEqual(uc, rootElement.Children.First());
        Assert.AreEqual(fcToe1, rootElement.Children.Last());
    }
    
    [TestMethod]
    public void SequenceTest()
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
    
    [TestMethod]
    public void SequencePreserveTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var rootElement = new RootSyntaxElement();
        var uc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));
        var fc = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias));
        var fcToe2 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc2Alias));
        var fcToe1 = new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.fcAlias), SimpleTestSyntaxModel.defaultArrow, new UmlFigure(UmlFigureType.Actor, SimpleTestSyntaxModel.uc1Alias));

        rootElement.Make(uc).Last();
        rootElement.Make(fc).Last();
        rootElement.Make(fcToe2).Last();
        rootElement.Make(fcToe1).Last();

        var refactoringManager = new DefaultRefactoringManager(builder);
        refactoringManager.Refactor(rootElement);

        var children = rootElement.Children.ToArray();
        Assert.AreEqual(4, children.Length);
        Assert.AreEqual(uc, children[0]);
        Assert.AreEqual(fc, children[1]);
        Assert.AreEqual(fcToe1, children[2]);
        Assert.AreEqual(fcToe2, children[3]);
    }
}