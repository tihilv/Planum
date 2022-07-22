using System;
using System.Collections.Generic;
using System.Threading;
using Bundle.Uml;
using Engine.PlantUml;
using Language.Common;
using Language.Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SvgVisualizer;

namespace Visualize.Tests;

[TestClass]
public class VisualizeTests
{
    [TestMethod]
    public void TestSimpleModel()
    {
        var script = new TextScript(GetSimpleScript());
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var documentModel = new DocumentModel(script, builder);
        var engine = new PlantUmlEngine();
        var drawingModel = new DrawingModel(documentModel, engine);
        bool finalized = false;
        var result = drawingModel.Document.Subscribe(document =>
        {
            if (document != null)
                finalized = true;
        });
        DateTime begin = DateTime.Now;
        while (!finalized && (DateTime.Now - begin).TotalMilliseconds < 2000)
            Thread.Sleep(100);
        
        Assert.IsTrue(finalized);
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