using IDE.PQLParser;

namespace Testing;

public static class Extensions
{
    public static string Normalize(this string str)
    {
        string res = string.Join(",", str.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).OrderBy(x => x));
        if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res)) return "none";
        return res;
    }

    public static string ParseWithExceptions(this QueryParser queryParser, string query)
    {
        try
        {
            return queryParser.ParseQuery(query);
        }
        catch (Exception e)
        {
            return "none";
        }
    }
}
