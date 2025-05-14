using System;
using System.Text.RegularExpressions;
using IDE.PQLParser;

namespace IDE.PQLParser;

public class QueryLexer
{
    private static readonly Dictionary<string, QueryKeywordType> Keywords = new()
    {
        {"Select", QueryKeywordType.Select},
        {"such that", QueryKeywordType.SuchThat},
        {"Follows", QueryKeywordType.Follows},
        {"Follows*", QueryKeywordType.FollowsT},
        {"Parent", QueryKeywordType.Parent},
        {"Parent*", QueryKeywordType.ParentT},
        {"Uses", QueryKeywordType.Uses},
        {"Uses*", QueryKeywordType.UsesT},
        {"Modifies", QueryKeywordType.Modifies},
        {"Modifies*", QueryKeywordType.ModifiesT},
        {"Calls", QueryKeywordType.Calls},
        {"Calls*", QueryKeywordType.CallsT},
        {"procedure", QueryKeywordType.Procedure},
        {"call", QueryKeywordType.Call},
        {"stmt", QueryKeywordType.Statement},
        {"assign", QueryKeywordType.Assign},
        {"while", QueryKeywordType.While},
        {"variable", QueryKeywordType.Variable},
        {"constant", QueryKeywordType.Constant},
        {"prog_line", QueryKeywordType.Prog_line},
        {"_", QueryKeywordType.Joker},
        {"with", QueryKeywordType.With},
        {"and", QueryKeywordType.And},
    };
    private static readonly Regex QueryKeywordRegex = new(@"""[^""]*""|\w+\.(procName|varName|value|stmt#)|such that|\w+\*?|_|,|\(|\)|=");

    public List<QueryKeyword> Tokenize(string query)
    {
        var queryTokens = new List<QueryKeyword>();

        foreach (Match match in QueryKeywordRegex.Matches(query))
        {
            string value = match.Value;
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.String, value.Trim('"')));
                //Console.WriteLine($"Lexer: new {queryTokens.Last().Type}: {queryTokens.Last().Value}");
            }
            else if (value.Contains("."))
            {
                var parts = value.Split('.');
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Identifier, parts[0]));
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Attribute, parts[1]));
                //Console.WriteLine("Lexer: new attribute with identifier: " + queryTokens.Last().Value);
            }
            else if (Keywords.TryGetValue(value, out QueryKeywordType type))
            {
                queryTokens.Add(new QueryKeyword(type, value));
                //Console.WriteLine("Lexer: new keyword: " + queryTokens.Last().Type);
            }
            else if (int.TryParse(value, out _))
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Number, value));
                //Console.WriteLine("Lexer: new number: " + queryTokens.Last().Value);

            }
            else if (value == "(")
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.OpenParen, value));
                //Console.WriteLine("Lexer: new character: " + queryTokens.Last().Value);

            }
            else if (value == ")")
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.CloseParen, value));
                //Console.WriteLine("Lexer: new character: " + queryTokens.Last().Value);

            }
            else if (value == ",")
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Comma, value));
                //Console.WriteLine("Lexer: new character: " + queryTokens.Last().Value);
            }
             else if (value == "=")
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Equals, value));
                //Console.WriteLine("Lexer: new equation: " + queryTokens.Last().Value);
            }
            else
            {
                queryTokens.Add(new QueryKeyword(QueryKeywordType.Identifier, value));
                //Console.WriteLine("Lexer: new identifier: " + queryTokens.Last().Value);
            }
        }
        queryTokens.Add(new QueryKeyword(QueryKeywordType.End, "EOF"));
        //Console.WriteLine("Lexer: new eof: " + queryTokens.Last().Value);
        return queryTokens;
    }
}
