﻿using System.Linq;
using Bundle.Uml.Elements;
using Bundle.Uml.Parsers;
using Bundle.Uml.Tokenizer;
using Language.Api;
using Language.Common.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class UmlParserTests
{
    [TestMethod]
    public void SingleStereotypeTest()
    {
        string value = "User << Human >>";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        var umlElement = UmlElementParser.Instance.Parse(tokens)?.SyntaxElement as UmlSyntaxElement;
        Assert.IsNotNull(umlElement);
        Assert.IsNotNull(umlElement.FirstFigure);
        Assert.IsNull(umlElement.Arrow);
        Assert.IsNull(umlElement.SecondFigure);
        Assert.IsNull(umlElement.Comment);
        Assert.AreEqual(new UmlFigure(UmlFigureType.NotDefined, null, "Human", "User"),umlElement.FirstFigure);
    }
    
    [TestMethod]
    public void SingleAliasStereotypeAndUrlTest()
    {
        string value = "(Use the application) as (Use) << Main >> [[http://google.com]]";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        var umlElement = UmlElementParser.Instance.Parse(tokens)?.SyntaxElement as UmlSyntaxElement;
        Assert.IsNotNull(umlElement);
        Assert.IsNotNull(umlElement.FirstFigure);
        Assert.IsNull(umlElement.Arrow);
        Assert.IsNull(umlElement.SecondFigure);
        Assert.IsNull(umlElement.Comment);
        Assert.AreEqual(new UmlFigure(UmlFigureType.UseCase, "Use the application", "Main", "(Use)","http://google.com"),umlElement.FirstFigure);
    }
    
    [TestMethod]
    public void ArrowWithCommentTest()
    {
        string value = "User --> (Use the application) : A small label";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        var umlElement = UmlElementParser.Instance.Parse(tokens)?.SyntaxElement as UmlSyntaxElement;
        Assert.IsNotNull(umlElement);
        Assert.IsNotNull(umlElement.FirstFigure);
        Assert.IsNotNull(umlElement.Arrow);
        Assert.IsNotNull(umlElement.SecondFigure);
        Assert.IsNotNull(umlElement.Comment);
        Assert.AreEqual(new UmlFigure(UmlFigureType.NotDefined, alias: "User"),umlElement.FirstFigure);
        Assert.AreEqual(new Arrow(ArrowShape.No, ArrowShape.Arrow, 2, LineType.Plain, Direction.Undefined),umlElement.Arrow);
        Assert.AreEqual(new UmlFigure(UmlFigureType.UseCase, "Use the application"),umlElement.SecondFigure);
    }
    
    [TestMethod]
    public void SingleNotDefinedTypeTest()
    {
        string value = "fc --> uc1";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        var umlElement = UmlElementParser.Instance.Parse(tokens)?.SyntaxElement as UmlSyntaxElement;
        Assert.IsNotNull(umlElement);
        Assert.IsNotNull(umlElement.FirstFigure);
        Assert.AreEqual(new UmlFigure(UmlFigureType.NotDefined, alias: "fc"),umlElement.FirstFigure);
    }
}