using IDE.Parser;
using IDE.PQLParser;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System;

namespace TestProject1;

[TestClass]
public sealed class Test1
{
    private static QueryParser queryParser;

    [ClassInitialize]
    public static void ClassSetup(TestContext ctx)
    {
        CodeParser parser = new CodeParser("C:\\Users\\janof\\source\\repos\\fBorovy\\ATS-Project\\IDE\\SIMPLE.txt");
        parser.ReadFile();
        parser.Parse();
        queryParser = new QueryParser();
    }

    [DataTestMethod]
    [DataRow("stmt s, s1; Select s such that Parent (s, s1) with s1.stmt#=2", "none")]
    [DataRow("stmt s, s1; Select s such that Parent (s, s1) with s1.stmt#=10", "6")]
    [DataRow("stmt s, s1; Select s such that Parent (s, s1) with s1.stmt#=11", "6")]
    [DataRow("stmt s, s1; Select s such that Parent (s, s1) with s1.stmt#=20", "16")]
    [DataRow("stmt s; Select s such that Parent (s, 2)", "none")]
    [DataRow("stmt s; Select s such that Parent (s, 10)", "6")]
    [DataRow("stmt s; Select s such that Parent (s, 11)", "6")]
    [DataRow("stmt s; Select s such that Parent (s, 20)", "16")]
    [DataRow("stmt s; Select s such that Parent (2, s)", "none")]
    [DataRow("stmt s; Select s such that Parent (8, s)", "none")]
    [DataRow("stmt s; Select s such that Parent (9,s)", "none")]
    [DataRow("stmt s; Select s such that Parent (25,s)", "none")]
    [DataRow("stmt s; Select s such that Parent* (s,2)", "none")]
    [DataRow("stmt s; Select s such that Parent* (s,10)", "6")]
    [DataRow("stmt s; Select s such that Parent* (s,11)", "6")]
    [DataRow("stmt s; Select s such that Parent* (s,20)", "6,12,14,15,16")]
    [DataRow("stmt s; while w; Select w such that Parent* (s, 2)", "none")]
    [DataRow("stmt s; while w; Select w such that Parent* (s, 10)", "6")]
    [DataRow("stmt s; while w; Select w such that Parent* (s, 11)", "6")]
    [DataRow("stmt s; while w; Select w such that Parent* (s, 20)", "6,12,16")]
    [DataRow("while w; Select w such that Parent* (w, 9)", "6")]
    [DataRow("while w; Select w such that Parent* (w, 11)", "6")]
    [DataRow("while w; Select w such that Parent*(w, 13)", "6,12")]
    [DataRow("while w; Select w such that Parent*(w, 21)", "6,12,16")]
    [DataRow("stmt s; Select s such that Follows(s, 1)", "2")]
    [DataRow("stmt s; Select s such that Follows(s, 8)", "9")]
    [DataRow("stmt s; Select s such that Follows(s, 9)", "10")]
    [DataRow("stmt s; Select s such that Follows(s, 10)", "11")]
    [DataRow("stmt s; Select s such that Follows(s, 12)", "118")]
    [DataRow("stmt s; Select s such that Follows(s, 13)", "14")]
    [DataRow("stmt s; Select s such that Follows(s, 23)", "29")]
    [DataRow("assign a; Select a such that Follows(a, 1)", "2")]
    [DataRow("assign a; Select a such that Follows(a, 8)", "9")]
    [DataRow("assign a; Select a such that Follows(a, 9)", "10")]
    [DataRow("while w; Select w such that Parent*(w, 13)", "6,12")]
    [DataRow("while w; Select w such that Parent*(w, 21)", "6,12,16")]
    [DataRow("stmt s; Select s such that Follows(s, 1)", "2")]
    [DataRow("stmt s; Select s such that Follows(s, 8)", "9")]
    [DataRow("stmt s; Select s such that Follows(s, 9)", "10")]
    [DataRow("stmt s; Select s such that Follows(s, 10)", "11")]
    [DataRow("stmt s; Select s such that Follows(s, 12)", "118")]
    [DataRow("stmt s; Select s such that Follows(s, 13)", "14")]
    [DataRow("stmt s; Select s such that Follows(s, 23)", "29")]
    [DataRow("assign a; Select a such that Follows(a, 1)", "2")]
    [DataRow("assign a; Select a such that Follows(a, 8)", "9")]
    [DataRow("assign a; Select a such that Follows(a, 9)", "10")]
    [DataRow("assign a; Select a such that Follows(a, 10)", "11")]
    [DataRow("assign a; Select a such that Follows(a, 12)", "none")]
    [DataRow("assign a; Select a such that Follows(a, 13)", "none")]
    [DataRow("while w; stmt s; Select w such that Follows*(s, w)", "12")]
    [DataRow("while w; stmt s; Select w such that Follows*(w, s)", "12,16,26,29,47,59,69,79,83,89,95,101,105")]
    [DataRow("stmt s; Select s such that Follows*(s, 1)", "2,3,4,5,6,118,119")]
    [DataRow("stmt s; Select s such that Follows*(s, 8)", "9,10,11,12,118")]
    [DataRow("stmt s; Select s such that Follows*(s, 9)", "10,11,12,118")]
    [DataRow("stmt s; Select s such that Follows*(s, 13)", "14,95,101")]
    [DataRow("stmt s; Select s such that Follows*(s, 19)", "20,21,22,23,29,34,45,54,55,59,62")]
    [DataRow("stmt s; Select s such that Follows*(s, 22)", "23,29,34,45,54,55,59,62")]
    [DataRow("if ifstat; Select ifstat such that Follows*(ifstat, 8)", "14,23,34,55,66,76,80,86,97,107,109")]
    [DataRow("if ifstat; Select ifstat such that Follows*(ifstat, 17)", "23,34,55")]
    [DataRow("if ifstat; Select ifstat such that Follows*(ifstat, 25)", "34,55")]
    [DataRow("if ifstat; Select ifstat such that Follows*(ifstat, 27)", "34,55")]
    [DataRow("assign a; Select a such that Follows*(a, 6)", "none")]
    [DataRow("assign a; Select a such that Follows*(a, 9)", "10,11")]
    [DataRow("assign a; Select a such that Follows*(a, 10)", "11")]
    [DataRow("assign a; Select a such that Follows*(a, 17)", "19,20,21,24,25,27,28,30,31,32,35,36,37,39,40,43,46,48,49,50,52,53,56,57,58,60,61")]
    [DataRow("assign a; Select a such that Follows*(a, 28)", "30,31,32,35,36,37,39,40,43,46,48,49,50,52,53,56,57,58,60,61")]
    [DataRow("variable v; Select v such that Modifies(3, v)", "height")]
    [DataRow("variable v; Select v such that Modifies(4, v)", "tmp")]
    [DataRow("variable v; Select v such that Modifies(6, v)", "x1")]
    [DataRow("variable v; Select v such that Modifies(18, v)", "blue,depth,edge,green,line,notmove,pixel,pink,semi,temporary,total")]
    [DataRow("variable v; Select v such that Modifies(24, v)", "y1")]
    [DataRow("variable v; Select v such that Modifies(28, v)", "y2")]
    [DataRow("while w; Select w such that Modifies(w, \"difference\")", "6,12,16")]
    [DataRow("while w; Select w such that Modifies(w, \"height\")", "6,12,16,59")]
    [DataRow("variable v; Select v such that Modifies(\"Main\", v)", "area,difference,height,tmp")]
    [DataRow("stmt s; Select s such that Uses(s, \"difference\")", "13,16,18,19,20,35,37,67,79")]
    [DataRow("stmt s; Select s such that Uses(s, \"height\")", "9,11,40,46")]
    [DataRow("variable v; Select v such that Uses(10, v)", "bottom,incre,y1")]
    [DataRow("variable v; Select v such that Uses(18, v)", "depth,difference,dot,edge,half,increase,notmove,pixel,semi,temporary")]
    [DataRow("variable v; Select v such that Uses(23, v)", "tmp")]
    [DataRow("assign a; variable v; Select v such that Uses(a, v)", "area,bottom,decrement,difference,height,I,incre,j,k,left,length,radius,right,top,tmp,volume,width,x1,x2,x3,x4,x5,x9,y1,y2")]
    [DataRow("assign a; Select a such that Modifies(a, \"area\") and Uses(a, \"area\")", "none")]
    [DataRow("assign a; Select a such that Modifies(a, \"difference\") and Uses(a, \"difference\")", "20")]
    [DataRow("assign a; Select a such that Modifies(a, \"x2\") and Uses(a, \"x2\")", "none")]
    [DataRow("assign a; Select a such that Modifies(a, \"height\") and Uses(a, \"height\")", "none")]
    [DataRow("while w; assign a; Select a such that Modifies(a, \"tmp\") and Parent(w, a)", "17,48,60")]
    [DataRow("while w; assign a; Select a such that Parent(w, a) and Modifies(a, \"tmp\")", "17,48,60")]
    [DataRow("while w; assign a; Select a such that Modifies(a, \"tmp\") and Parent(w, a)", "17,48,60")]
    [DataRow("procedure p; Select p such that Calls*(p, \"Show\")", "Enlarge,Main,Random")]
    [DataRow("procedure p; Select p such that Calls(\"Main\", p) and Modifies(p, \"height\") and Uses(p, \"area\")", "none")]
    [DataRow("procedure p; Select p such that Calls*(\"Main\", p) and Modifies(p, \"height\")", "none")]
    [DataRow("assign a; Select a pattern a (\"difference\", _)", "13,20,37")]
    [DataRow("assign a; Select a pattern a (\"tmp\", _\"3*area\")", "none")]
    [DataRow("while w; assign a; Select a pattern a (\"area\", _) such that Follows(w, a)", "none")]
    [DataRow("assign a; Select a pattern a (_, \"difference+1\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"difference*5+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"+difference+k+x2+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"tmp+area+difference+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"tmp+difference+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"k*difference+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"difference+3\")", "35")]
    [DataRow("assign a; Select a pattern a (_, \"+3*area+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"5*3+\")", "none")]
    [DataRow("assign a; Select a pattern a (_, \"difference-1\")", "20")]
    public void KamilTest(string query, string expectedResult)
    {
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult, result.Trim());
    }
}
