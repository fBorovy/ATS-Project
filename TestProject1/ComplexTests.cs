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
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
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
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
    [DataRow("", "")]
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
