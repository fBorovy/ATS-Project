using System;

namespace IDE.PQLParser;

public class QueryKeyword
{
    public QueryKeywordType Type;
    public string Value { get; }
    
    public QueryKeyword(QueryKeywordType type, string value)
    {
        Type = type;
        Value = value;
    } 
}
