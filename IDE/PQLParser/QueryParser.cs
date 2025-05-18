namespace IDE.PQLParser;

public class QueryParser
{
    private readonly QueryLexer _lexer;
    private readonly QueryPreprocessor _preprocessor;
    private readonly QueryEvaluator _queryEvaluator;

    public QueryParser()
    {
        _lexer = new QueryLexer();
        _preprocessor = new QueryPreprocessor();
        _queryEvaluator = new QueryEvaluator();
    }


    public string ParseQuery(string query) {
        List<QueryKeyword> currentQuery = _lexer.Tokenize(query);
        //_preprocessor.ValidateQuery(currentQuery);
        QueryTree tree = _preprocessor.BuildQueryTree(currentQuery);
        string result = _queryEvaluator.EvaluateQuery(tree);
        return string.IsNullOrEmpty(result) ? "none" : result;
    }


}
