using System;
using System.Collections.Generic;
using System.Linq;
using Bundle.Uml;
using Bundle.Uml.Elements;
using Language.Common;
using Language.Common.Parsers;
using Language.Common.Primitives;
using Language.Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class ScriptParsingTests
{
    [TestMethod]
    public void SimpleProcessingTest()
    {
        var sb = GetSimpleScript();
        var script = new TextScript(sb);
        
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);
        var interpreter = new ScriptInterpreter(script, builder);
        interpreter.UpdateSyntaxModel();

        var umlRoot = (UmlRootSyntaxElement)interpreter.RootSyntaxElement.Children.Single();
        Assert.AreEqual(6, umlRoot.Children.Count);
        var rootChildren = umlRoot.Children.ToArray();
        Assert.AreEqual(PlantDirection.LeftToRight, ((DirectionSyntaxElement)rootChildren[0]).Direction);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "Food Critic", null, "fc")), (UmlSyntaxElement)rootChildren[1]);
        
        Assert.AreEqual(UmlContainerType.Rectangle, ((UmlContainerSyntaxElement)rootChildren[2]).Type);
        Assert.AreEqual("Restaurant", ((UmlContainerSyntaxElement)rootChildren[2]).Name);
        Assert.AreEqual(3, ((UmlContainerSyntaxElement)rootChildren[2]).Children.Count);
        var restaurantChildren = ((UmlContainerSyntaxElement)rootChildren[2]).Children.ToArray();
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, "Eat Food", null, "UC1")), (UmlSyntaxElement)restaurantChildren[0]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, "Pay for Food", null, "UC2")), (UmlSyntaxElement)restaurantChildren[1]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.UseCase, "Drink", null, "UC3")), (UmlSyntaxElement)restaurantChildren[2]);
        
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "UC1")), (UmlSyntaxElement)rootChildren[3]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "UC2")), (UmlSyntaxElement)rootChildren[4]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Actor, "UC3")), (UmlSyntaxElement)rootChildren[5]);
    }
    
    [TestMethod]
    public void SimpleSynthesizeTest()
    {
        var sb = GetSimpleScript();
        var script = new TextScript(sb);
        
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);
        var interpreter = new ScriptInterpreter(script, builder);
        interpreter.UpdateSyntaxModel();

        interpreter.UpdateScript();
        var resultLines = script.GetLines().ToArray();
        Assert.AreEqual(sb.Count, resultLines.Length);
        Assert.IsTrue(Enumerable.SequenceEqual(sb, resultLines.Select(r=>r.Value)));
    }

    private static List<String> GetSimpleScript()
    {
        var sb = new List<String>();
        sb.Add("@startuml");
        sb.Add("left to right direction");
        sb.Add("actor \"Food Critic\" as fc");
        sb.Add("rectangle Restaurant {");
        sb.Add("    usecase \"Eat Food\" as UC1");
        sb.Add("    usecase \"Pay for Food\" as UC2");
        sb.Add("    usecase \"Drink\" as UC3");
        sb.Add("}");
        sb.Add("fc --> UC1");
        sb.Add("fc --> UC2");
        sb.Add("fc --> UC3");
        sb.Add("@enduml");
        return sb;
    }
}