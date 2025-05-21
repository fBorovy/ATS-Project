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
    [DataRow("stmt s;Select s such that Parent(s, 25)", "23")]
    [DataRow("procedure p;Select p such that Calls(\"Init\", p) ", "none")]
    [DataRow("variable v;Select v such that Uses (20, v)", "difference")]
    [DataRow("stmt s,s1;Select s such that Parent(s, s1) with s1.stmt#= 12", "6")]
    [DataRow("stmt s;Select s such that Follows(s, 9)", "8")]
    [DataRow("if ifs; Select ifs such that Parent*(ifs, 185)", "184, 181, 180")]
    [DataRow("procedure p;Select p such that Modifies (p, \"x1\")", "Main, Init, Transform, Shear, Move, Shrink")]
    [DataRow("variable v;Select v such that Uses (10, v)", "y1, incre, bottom")]
    [DataRow("procedure p;Select p such that Modifies (p, \"top\")", "Main, Init, Shift")]
    [DataRow("if ifs;Select ifs such that Parent (ifs, 16)", "15")]
    [DataRow("stmt s;Select s such that Uses(s, \"y\")", "none")]
    [DataRow("while w; Select w such that Parent(w, 10)", "6")]
    [DataRow("if ifstmt;Select ifstmt such that Follows*(ifstmt, 7)", "none")]
    [DataRow("variable v;Select v such that Modifies (4, v)", "tmp")]
    [DataRow("while w;Select w such that Parent* (w, 13)", "12, 6")]
    [DataRow("procedure p; Select BOOLEAN such that Calls(p, \"Scale\")", "FALSE")]
    [DataRow("stmt s,s1;Select s such that Parent(s, s1) with s1.stmt#= 28", "26")]
    [DataRow("assign a;Select a such that Modifies(a, \"x4\")", "153")]
    [DataRow("assign a;Select a such that Follows(a, 8)", "7")]
    [DataRow("stmt s;Select s such that Follows* (s, 19)", "17, 18")]
    [DataRow("procedure p;Select p such that Calls(p, \"Draw\")", "Main, Shrink")]
    [DataRow("stmt s; Select s such that Follows*(1, s)", "2, 3, 4, 5, 6, 119")]
    [DataRow("stmt s; Select s such that Follows*(119, s)", "none")]
    [DataRow("stmt s;Select s such that Parent* (s, 30)", "29, 16, 15, 14, 12, 6")]
    [DataRow("stmt s; Select s such that Parent(s, 221)", "218")]
    [DataRow("procedure p; Select p such that Modifies(p, \"x3\")", "Main, Shift")]
    [DataRow("assign a;Select a such that Modifies(a, \"reduce\")", "none")]
    [DataRow("procedure p;Select p such that Calls(p, \"Draw\")", "Main, Shrink")]
    [DataRow("stmt s; Select s such that Parent* (s, 10)", "6")]
    [DataRow("stmt s, s1; Select s such that Follows*(s, s1) with s1.stmt# = 6", "1, 2, 3, 4, 5")]

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
    [DataRow("stmt s; Select s such that Follows(1, s)", "2")]
    [DataRow("stmt s; Select s such that Follows(2, s)", "3")]
    [DataRow("stmt s; Select s such that Follows(3, s)", "4")]
    [DataRow("stmt s; Select s such that Follows(s, 3)", "2")]
    [DataRow("stmt s; Select s such that Follows(s, 2)", "1")]
    [DataRow("stmt s; Select s such that Parent(6, s)", "7")]
    [DataRow("stmt s; Select s such that Parent(12, s)", "13")]
    [DataRow("stmt s; Select s such that Parent(s, 14)", "13")]
    [DataRow("stmt s; Select s such that Parent*(12, s)", "16")]
    [DataRow("stmt s; Select s such that Parent(6, s)", "7")]
    [DataRow("stmt s; Select s such that Modifies(2, \"width\")", "2")]
    [DataRow("stmt s; Select s such that Modifies(4, \"tmp\")", "4")]
    [DataRow("stmt s; Select s such that Modifies(s, \"height\")", "3")]
    [DataRow("procedure p; Select p such that Modifies(p, \"width\")", "Main")]
    [DataRow("procedure p; Select p such that Modifies(p, \"decrement\")", "Init")]
    [DataRow("stmt s; Select s such that Uses(9, \"top\")", "9")]
    [DataRow("stmt s; Select s such that Uses(8, \"x1\")", "8")]
    [DataRow("stmt s; Select s such that Uses(31, \"right\")", "31")]
    [DataRow("stmt s; Select s such that Uses(s, \"height\")", "11")]
    [DataRow("procedure p; Select p such that Uses(p, \"height\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(\"Main\", p)", "Init Random")]
    [DataRow("procedure p; Select p such that Calls(p, \"Init\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Random\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Draw\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Translate\")", "Main")]
    [DataRow("stmt s; Select s such that Affects(30, s)", "32")]
    [DataRow("stmt s; Select s such that Affects(s, 50)", "46")]
    [DataRow("stmt s; Select s such that Affects(46, 50)", "50")]
    [DataRow("stmt s; Select s such that Affects(1, s)", "")]
    [DataRow("stmt s; Select s such that Affects(32, s)", "")]
    public void MaciekTests(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }
}
