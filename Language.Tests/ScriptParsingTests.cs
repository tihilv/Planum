using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.NotDefined, alias: "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.NotDefined, alias: "UC1")), (UmlSyntaxElement)rootChildren[3]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.NotDefined, alias: "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.NotDefined, alias: "UC2")), (UmlSyntaxElement)rootChildren[4]);
        Assert.AreEqual(new UmlSyntaxElement(new UmlFigure(UmlFigureType.NotDefined, alias: "fc"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.NotDefined, alias: "UC3")), (UmlSyntaxElement)rootChildren[5]);
    }
    
    [TestMethod]
    public void ExistingSynthesizeTest()
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

    [TestMethod]
    public void NewSynthesizeTest()
    {
        var sb = new []{"@startuml", "@enduml"};
        var script = new TextScript(sb);
        
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);
        var interpreter = new ScriptInterpreter(script, builder);
        interpreter.UpdateSyntaxModel();


        var umlRoot = (UmlRootSyntaxElement)interpreter.RootSyntaxElement.Children.Single();
        umlRoot.Make(new DirectionSyntaxElement(PlantDirection.TopToBottom)).Last();
        var container = new UmlContainerSyntaxElement(UmlContainerType.Package, "MyPackage");
        umlRoot.Make(container).Last();

        container.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "Actor1"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 3, LineType.Dash, Direction.Right), new UmlFigure(UmlFigureType.Component, "Component1"))).Last();
        container.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "Actor1"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 2), new UmlFigure(UmlFigureType.Component, "Component2"))).Last();
        
        interpreter.UpdateScript();
        var resultLines = script.GetLines().ToArray();
        Assert.AreEqual(7, resultLines.Length);
        
        var ethalon = new List<String>();
        ethalon.Add("@startuml");
        ethalon.Add(" top to bottom direction");
        ethalon.Add(" package \"MyPackage\" {");
        ethalon.Add("  actor \"Actor1\" .r..> component \"Component1\"");
        ethalon.Add("  actor \"Actor1\" --> component \"Component2\"");
        ethalon.Add(" }");
        ethalon.Add("@enduml");
        Assert.IsTrue(Enumerable.SequenceEqual(ethalon, resultLines.Select(r=>r.Value)));
    }
    
    [TestMethod]
    public void SecondLineAliasSynthesizeTest()
    {
        var sb = new []{"@startuml", "@enduml"};
        var script = new TextScript(sb);
        
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);
        var interpreter = new ScriptInterpreter(script, builder);
        interpreter.UpdateSyntaxModel();

        var umlRoot = (UmlRootSyntaxElement)interpreter.RootSyntaxElement.Children.Single();

        umlRoot.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.Actor, "Actor1", alias:"a1"))).Last();
        umlRoot.Make(new UmlSyntaxElement(new UmlFigure(UmlFigureType.NotDefined, alias: "a1"), new Arrow(ArrowShape.No, ArrowShape.Arrow, 3, LineType.Dash, Direction.Right), new UmlFigure(UmlFigureType.Component, "Component1"))).Last();
        
        interpreter.UpdateScript();
        var resultLines = script.GetLines().ToArray();
        Assert.AreEqual(4, resultLines.Length);
        
        var ethalon = new List<String>();
        ethalon.Add("@startuml");
        ethalon.Add(" actor \"Actor1\" as a1");
        ethalon.Add(" a1 .r..> component \"Component1\"");
        ethalon.Add("@enduml");
        Assert.IsTrue(Enumerable.SequenceEqual(ethalon, resultLines.Select(r=>r.Value)));
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
    
    [TestMethod]
    public void ProcessingPerformanceTest()
    {
        var sb = GetBigScript();
        var script = new TextScript(sb);
        
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var sw = Stopwatch.StartNew(); 
        var interpreter = new ScriptInterpreter(script, builder);
        interpreter.UpdateSyntaxModel();
        sw.Stop();
        Assert.IsTrue(sw.ElapsedMilliseconds < 200);
        
        sw.Restart();
        interpreter.UpdateScript();
        sw.Stop();
        Assert.IsTrue(sw.ElapsedMilliseconds < 200);
    }
    
    [TestMethod]
    public void SynthesisPerformanceTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var sb = GetBigScript();
        var script = new TextScript(sb);
        var interpreter = new ScriptInterpreter(script, builder);

        interpreter.UpdateSyntaxModel();
        var root = interpreter.RootSyntaxElement.Children.Single();
        interpreter.RootSyntaxElement.Make(root).Deleted();
        
        var newScript = new TextScript(Array.Empty<string>());
        var newInterpreter = new ScriptInterpreter(newScript, builder);
        newInterpreter.UpdateSyntaxModel();
        
        newInterpreter.RootSyntaxElement.Make(root).First();

        var sw = Stopwatch.StartNew();
        newInterpreter.UpdateScript();
        sw.Stop();
        Assert.IsTrue(sw.ElapsedMilliseconds < 200);
    }

    internal static List<String> GetBigScript()
    {
        int groupCount = 100;
        int elementsPerGroup = 100;
        var sb = new List<String>();
        sb.Add("@startuml");
        for (int i = 0; i < groupCount; i++)
        {
            sb.Add($"actor \"Food Critic {i}\" as fc{i}");
            
            sb.Add($"rectangle \"Restaurant {i}\" {{");
            for (int j = 0; j < elementsPerGroup; j++)
                sb.Add($"    usecase \"Eat Food {i} {j}\" as UC{i}_{j}");
            sb.Add("}");

            for (int j = 0; j < elementsPerGroup; j++)
                sb.Add($"fc{i} --> UC{i}_{j}");
        }
        sb.Add("@enduml");
        return sb;
    }

}