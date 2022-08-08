using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Bundle.Uml;
using Bundle.Uml.Semantic;
using Language.Api.Semantic;
using Language.Common;
using Language.Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SvgVisualizer;
using VectorDrawing;
using Visualize.Api;
using Visualize.Api.Geometry;

namespace Visualize.Tests;

[TestClass]
public class VisualizeTests
{
    [TestMethod]
    public void ImageGenerationTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var documentModel = new DocumentModel(new TextScript(GetSimpleScript()), builder);
        var vectorImage = new VectorImage();
        var pipeline = new DocumentToImageSvgPipelineFactory().Create(documentModel, vectorImage);
        
        bool finalized = false;
        pipeline.Changed += (sender, args) =>
        {
            finalized = true;
        };
        DateTime begin = DateTime.Now;
        while (!finalized && (DateTime.Now - begin).TotalMilliseconds < 2000)
            Thread.Sleep(100);
        
        Assert.IsTrue(finalized);
        Refresh(vectorImage);
    }
    
    private void Refresh(VectorImage vectorImage)
    {
        using (var bitmap = new Bitmap(1500, 1500))
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                var modelRectangle = vectorImage.GetBoundaries();
                vectorImage.Rasterize(modelRectangle, new RectangleF(0, 0, 1500, 1500), g);
            }

            bitmap.Save("d:\\111.bmp");
        }
    }
    
    [TestMethod]
    public void DrawingContextTest()
    {
        var modelRectangle = new RectangleD(10, 20, 200, 100);
        var graphicsRectangle = new RectangleF(300, 400, 20, 10);

        var drawingContext = new DrawingContext(modelRectangle, graphicsRectangle, (_, _, _) => { });
        var graphicsPt1 = drawingContext.ToGraphics(new PointD(10, 20));
        Assert.AreEqual(300, graphicsPt1.X);
        Assert.AreEqual(400, graphicsPt1.Y);
        
        var graphicsPt2 = drawingContext.ToGraphics(new PointD(210, 120));
        Assert.AreEqual(318, graphicsPt2.X);
        Assert.AreEqual(409, graphicsPt2.Y);

        var graphicsPt3 = drawingContext.ToGraphics(new PointD(110, 70));
        Assert.AreEqual(309, graphicsPt3.X);
        Assert.AreEqual(404.5, graphicsPt3.Y);
    }
    
    [TestMethod]
    public void SelectionTest()
    {
        var builder = new DefaultBundleBuilder();
        builder.RegisterBundle(UmlBundle.Instance);

        var documentModel = new DocumentModel(new TextScript(GetSimpleScript()), builder);
        var vectorImage = new VectorImage();
        var pipeline = new DocumentToImageSvgPipelineFactory().Create(documentModel, vectorImage);
        
        bool finalized = false;
        pipeline.Changed += (sender, args) =>
        {
            var selectedElement = pipeline.Select(new PointD(180, 120)) as UmlFigureSemanticElement;
            Assert.AreEqual("Pay for Food", selectedElement?.Text);
            finalized = true;
        };
        DateTime begin = DateTime.Now;
        while (!finalized && (DateTime.Now - begin).TotalMilliseconds < 2000)
            Thread.Sleep(100);
        
        Assert.IsTrue(finalized);
        Refresh(vectorImage);
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