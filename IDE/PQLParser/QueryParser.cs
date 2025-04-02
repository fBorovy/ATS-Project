using IDE.PQLParser;

namespace IDE.QueryParser;

public class QueryParser
{
    private readonly QueryLexer _lexer;
    private readonly QueryPreprocessor _preprocessor;

    public QueryParser(QueryLexer lexer, QueryPreprocessor preprocessor)
    {
        _lexer = lexer;
        _preprocessor = preprocessor;
    }


    public void ParseQuery(string query) {
        List<QueryKeyword> currentQuery = _lexer.Tokenize(query);
        //_preprocessor.ValidateQuery(currentQuery);
        _preprocessor.BuildQueryTree(currentQuery);
        
    }


}
