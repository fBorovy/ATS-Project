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
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
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
