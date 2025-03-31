using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Interfaces;
using Atsi.Structures.SIMPLE.Statements;

public class StatementAnalyzer : IStatementAnalyzer
{
    private readonly IExpressionAnalyzer expressionAnalyzer;

    public StatementAnalyzer(IExpressionAnalyzer expressionAnalyzer)
    {
        this.expressionAnalyzer = expressionAnalyzer;
    }

    public HashSet<string> GetModifiedVariables(Statement statement)
    {
        var result = new HashSet<string>();

        switch (statement)
        {
            case AssignStatement assign:
                result.Add(assign.VariableName);
                break;
        }

        return result;
    }

    public HashSet<string> GetUsedVariables(Statement statement)
    {
        var result = new HashSet<string>();

        switch (statement)
        {
            case AssignStatement assign:
                result.UnionWith(expressionAnalyzer.GetUsedVariables(assign.Expression));
                break;
            case WhileStatement @while:
                result.Add(@while.ConditionalVariableName);
                foreach (var stmt in @while.StatementsList)
                {
                    result.UnionWith(GetUsedVariables(stmt));
                }
                break;
        }

        return result;
    }
}
