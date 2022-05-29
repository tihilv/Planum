using System.Linq;
using Bundle.Uml.Tokenizer;
using Language.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Tests;

[TestClass]
public class UmlTokenizerTests
{
    [TestMethod]
    public void SingleTokenTest()
    {
        string value = "mydata";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(1, tokens.Length);
        Assert.AreEqual(line, tokens[0].ScriptLine);
        Assert.AreEqual(value, tokens[0].Value);
        Assert.AreEqual(new CharacterRange(0, (ushort)(value.Length - 1)), tokens[0].Range);
    }
    
    [TestMethod]
    public void SingleInSpacesTokenTest()
    {
        string value = "mydata";
        var line = new ScriptLine(1, $"  {value}   ");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(1, tokens.Length);
        Assert.AreEqual(line, tokens[0].ScriptLine);
        Assert.AreEqual(value, tokens[0].Value);
        Assert.AreEqual(new CharacterRange(2, (ushort)(value.Length + 1)), tokens[0].Range);
    }
    
    [TestMethod]
    public void MultipleTokensTest()
    {
        string value1 = "mydata1";
        string value2 = "mydata2";
        string value3 = "mydata3";
        var line = new ScriptLine(1, $"{value1} {value2}    {value3}");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual(value1, tokens[0].Value);
        Assert.AreEqual(value2, tokens[1].Value);
        Assert.AreEqual(value3, tokens[2].Value);
        
        Assert.AreEqual(new CharacterRange(0, 6), tokens[0].Range);
        Assert.AreEqual(new CharacterRange(8, 14), tokens[1].Range);
        Assert.AreEqual(new CharacterRange(19, 25), tokens[2].Range);
    }

    [TestMethod]
    public void SingleBracketedTokenTest()
    {
        string value = ":my data (in) braces:";
        var line = new ScriptLine(1, value);
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(1, tokens.Length);
        Assert.AreEqual(value, tokens[0].Value);
        Assert.AreEqual(new CharacterRange(0, (ushort)(value.Length - 1)), tokens[0].Range);
    }
    
    [TestMethod]
    public void SingleBracketedInSpacesTokenTest()
    {
        string value = ":my data in braces:";
        var line = new ScriptLine(1, $"   {value}  ");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(1, tokens.Length);
        Assert.AreEqual(value, tokens[0].Value);
        Assert.AreEqual(new CharacterRange(3, (ushort)(value.Length + 2)), tokens[0].Range);
    }

    [TestMethod]
    public void OpenBracketTokenTest()
    {
        string value1 = ":my data:";
        string value2 = "braces";
        var line = new ScriptLine(1, $"{value1} :{value2}");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(3, tokens.Length);
        Assert.AreEqual(value1, tokens[0].Value);
        Assert.AreEqual(":", tokens[1].Value);
        Assert.AreEqual(value2, tokens[2].Value);
        Assert.AreEqual(new CharacterRange(0, (ushort)(value1.Length - 1)), tokens[0].Range);
        Assert.AreEqual(new CharacterRange(10, 10), tokens[1].Range);
        Assert.AreEqual(new CharacterRange(11, 16), tokens[2].Range);
    }

    [TestMethod]
    public void OpenCloseBracesTest()
    {
        string value1 = "()";
        string value2 = "interface";
        var line = new ScriptLine(1, $"{value1}{value2}");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(2, tokens.Length);
        Assert.AreEqual(value1, tokens[0].Value);
        Assert.AreEqual(value2, tokens[1].Value);
    }
    
    [TestMethod]
    public void OpenCloseDoubleBracesWithSpacesTest()
    {
        string value1 = "Something";
        string value2 = "very interesting";
        var line = new ScriptLine(1, $"{value1} << {value2} >>");
        var tokens = UmlTokenizer.Instance.GetTokens(line).ToArray();
        Assert.AreEqual(2, tokens.Length);
        Assert.AreEqual(value1, tokens[0].Value);
        Assert.AreEqual($"<< {value2} >>", tokens[1].Value);
    }
}