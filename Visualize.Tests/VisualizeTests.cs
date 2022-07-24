using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Bundle.Uml;
using Language.Common;
using Language.Processing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SvgVisualizer;
using VectorDrawing;
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
                vectorImage.Rasterize(modelRectangle, new RectangleD(0, 0, 1500, 1500), g);
            }

            bitmap.Save("d:\\111.bmp");
        }
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