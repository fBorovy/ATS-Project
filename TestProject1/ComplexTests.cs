using IDE.Parser;
using IDE.PQLParser;

namespace TestProject1;

[TestClass]
public sealed class ComplexTests
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
    [DataRow("variable v; Select v such that Modifies(10, v) and Uses(12, v)", "y2")]
    [DataRow("stmt s; Select s such that Follows(2, s) and Modifies(s, \"height\")", "3")]
    [DataRow("while w; Select w such that Parent(w, 29) and Modifies(w, \"tmp\")", "16")]
    [DataRow("stmt s; Select s such that Parent(s, 186) and Follows*(s, 183)", "181")]
    [DataRow("while w; Select w such that Parent(w, 85) and Modifies(w, \"x\")", "83")]
    [DataRow("procedure p; Select p such that Uses(p, \"x4\") and Calls(p, \"Main\")", "none")]
    [DataRow("stmt s; Select s such that Follows(1, s) and Modifies(s, \"width\")", "2")]
    [DataRow("while w; Select w such that Parent(w, 50) and Modifies(w, \"length\")", "47")]
    [DataRow("stmt s; Select s such that Parent*(s, 14) and Next(s, 13)", "12, 6")]
    [DataRow("while w; Select w such that Follows(68, w) and Modifies(w, \"I\")", "69")]
    [DataRow("stmt s; if ifstmt; Select s such that Modifies(s, \"o\") and Follows*(ifstmt, s)", "none")]
    [DataRow("while w; Select w such that Parent(w, 91) and Follows(w, 88)", "89")]
    [DataRow("variable v; Select v such that Modifies(2, v) and Uses(11, v)", "width")]
    [DataRow("while w; Select w such that Parent(w, 31) and Modifies(w, \"height\")", "29")]
    [DataRow("procedure p; stmt s1, s2; Select p such that Modifies(p, \"x3\") and Next(s1, s2)", "Main, Shift")]
    [DataRow("stmt s; Select s such that Follows*(s, 133) and Uses(s, \"value\")", "none")]
    [DataRow("variable v; Select v such that Modifies(31, v) and Uses(32, v) and Next(31, 32)", "height")]
    [DataRow("stmt s; Select s such that Follows*(s, 85) and Parent*(79, s)", "84")]
    [DataRow("while w; Select w such that Parent(w, 257) and Follows(w, 256) and Next(256, 257)", "256")]
    [DataRow("stmt s; Select s such that Follows*(s, 36) and Parent*(34, s)", "35")]
    [DataRow("stmt s; Select s such that Follows(3, s) and Modifies(s, \"x\")", "4")]
    [DataRow("if ifstmt; Select ifstmt such that Parent(ifstmt, 87) and Follows*(ifstmt, 85)", "86")]
    [DataRow("stmt s; Select s such that Follows(130, s) and Next(130, s) and Modifies(s, \"right\")", "131")]
    [DataRow("if ifstmt; Select ifstmt such that Parent(ifstmt, 87) and Follows(ifstmt, 85)", "86")]
    [DataRow("stmt s; Select s such that Follows*(s, 253) and Parent*(251, s) and Next(s, 253)", "252")]
    [DataRow("while w; Select w such that Parent(w, 138) and Modifies(w, \"x1\") and Follows*(w, 139)", "136")]
    [DataRow("variable v; Select v such that Modifies(2, v) and Uses(9, v) and Next*(2, 9)", "width")]
    [DataRow("while w; Select w such that Parent(w, 137) and Follows(w, 135) and Next(135, w)", "136")]
    [DataRow("stmt s; Select s such that Follows(245, s) and Next(245, s) and Modifies(s, \"x2\")", "246")]
    [DataRow("if ifstmt; Select ifstmt such that Parent(ifstmt, 15) and Follows(13, ifstmt) and Next(13, ifstmt)", "14")]

    public void EwaTests(string query, string expectedResult)
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
