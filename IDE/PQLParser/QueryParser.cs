using IDE.PQLParser;

namespace IDE.QueryParser;

public class QueryParser
{
    private readonly QueryLexer _lexer;
    private readonly QueryPreprocessor _preprocessor;
    private readonly QueryEvaluator _queryEvaluator;

    public QueryParser(QueryLexer lexer, QueryPreprocessor preprocessor, QueryEvaluator evaluator)
    {
        _lexer = lexer;
        _preprocessor = preprocessor;
        _queryEvaluator = evaluator;
    }


    public String ParseQuery(string query) {
        List<QueryKeyword> currentQuery = _lexer.Tokenize(query);
        //_preprocessor.ValidateQuery(currentQuery);
        QueryTree tree = _preprocessor.BuildQueryTree(currentQuery);
        String result = _queryEvaluator.EvaluateQuery(tree);
        return result;
    }


}
