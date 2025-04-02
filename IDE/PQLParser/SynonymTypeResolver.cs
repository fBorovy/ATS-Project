using System;

namespace IDE.PQLParser;

public static class SynonymTypeResolver
{
    private static readonly Dictionary<string, SynonymType> KeywordMap = new()
    {
        { "assign", SynonymType.Assign },
        { "while", SynonymType.While },
        { "procedure", SynonymType.Procedure },
        { "stmt", SynonymType.Statement },
        { "variable", SynonymType.Variable },
        { "constant", SynonymType.Constant },
        { "prog_line", SynonymType.Prog_line },
    };

    public static bool TryParse(string keyword, out SynonymType type) =>
        KeywordMap.TryGetValue(keyword, out type);
}