using IDE.Parser;
using IDE.PQLParser;

namespace TestProject1;

[TestClass]
public sealed class SimpleTests
{
    private static QueryParser queryParser;

    [ClassInitialize]
    public static void ClassSetup(TestContext ctx)
    {
        CodeParser parser = new CodeParser($"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\SIMPLE.txt");
        parser.ReadFile();
        parser.Parse();
        queryParser = new QueryParser();
    }

    [DataTestMethod]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    public void KamilTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }


    [DataTestMethod]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    public void EwaTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }


    [DataTestMethod]
    [DataRow("assign a; while w;Select a such that Follows (w, a)","11, 46, 68, 82, 102, 135, 183, 186, 255, 263, 380")]
    [DataRow("stmt s, s1;Select s such that Parent (s, s1) with s1.stmt# = 77", "76")]
    [DataRow("call c;Select c such that Follows (c, 85)", "84")]
    [DataRow("call c, stmt s;Select c such that Follows* (c, s) with s1.stmt# = 6", "5")]
    [DataRow("if ifs, variable v;Select ifs such that Uses (ifs, \"volume\")", "55")]
    [DataRow("if ifs; while w;Select w such that Parent (if, w)", "16, 26, 79, 83, 89, 113, 217, 239, 251, 256, 279, 281")]
    [DataRow("if ifs; while w;Select ifs such that Parent (if, w) with ifs.stmt# = 15", "15")]
    [DataRow("procedure p, q;Select q such that Calls (p, q) with p.procName = \"Draw\"", "Clear, Random, Show")]
    [DataRow("stmt s;Select s such that Uses (s, \"height\")", "9, 11, 32, 40, 46, 86, 137, 148, 186, 263, 273")]
    [DataRow("stmt s;Select s such that Parent*(34, s)", "35,36,37,38,39,40,41,42,43,44")]
    [DataRow("variable v; procedure p;Select v such that Uses (p, v) with p.procName = \"Init\"", "120,121,122,123,124,125,126,127,128,129")]
    [DataRow("while w;Select w with w.stmt# = 143", "143")]
    [DataRow("assign a, if ifs;Select a such that Parent (ifs, a) with ifs.stmt# = 176", "177,178")]
    [DataRow("procedure p;Select p such that Uses (p, \"asterick\")", "Draw")]
    [DataRow("stmt s;Select s such that Uses (s, asterick)", "196,206")]
    [DataRow("assign a;Select a such that Modifies (a, \"blue\")", "215")]
    [DataRow("procedure p; Select p such that Calls (p, \"Show\")", "Draw,Enlarge,Rotate")]
    [DataRow("stmt s;Select s such that Uses (s, \"wrong\")", "278")]
    [DataRow("stmt s;Select s such that Follows* (s, 287)", "283,284,285,268")]
    [DataRow("stmt s;Sekect s such that Modifies (s, \"cs5\")", "290,298,302,311")]
    [DataRow("procedure p:Select p", "Main,Init,Random,Transform,Shift,Shear,Move,Draw,Clear,Show,Enlarge,Fill,Shrink,Translate,Rotate,Scale,PP,QQ,SS,TT,UU,XX")]
    [DataRow("while w; variable v;Select w such that Uses (w, \"top\")", "59,89")]
    [DataRow("assign a;Select a such that Modifies (a, \"top\")", "77,126,150")]
    [DataRow("assign a;Select a such that Uses (a, \"top\")", "9,67,131,135")]
    [DataRow("while w; stmt s;Select w such that Parent (w, s) with s.stmt# = 186", "181")]
    [DataRow("while w; stmt s;Select s such that Parent (w, s) with w.stmt# = 181", "182,183,184,186,187")]
    [DataRow("stmt s;Select s such that Parent (s, 100)", "97")]
    [DataRow("Select BOOLEAN such that Uses (8, \"jaja\")", "FALSE")]
    [DataRow("procedure p, q;Select BOOLEAN such that Calls(p,q)", "TRUE")]
    [DataRow("Select BOOLEAN such that Modifies(999, \"x\")", "FALSE")]
    public void FilipTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }


    [DataTestMethod]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    public void JanTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }


    [DataTestMethod]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    public void KasiaTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }


    [DataTestMethod]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    public void MaciekTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }
}
