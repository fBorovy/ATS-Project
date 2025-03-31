using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Interfaces;
using Atsi.Structures.SIMPLE.Statements;

public class ProcedureAnalyzer : IProcedureAnalyzer
{
    private readonly IStatementAnalyzer statementAnalyzer;

    public ProcedureAnalyzer(IStatementAnalyzer statementAnalyzer)
    {
        this.statementAnalyzer = statementAnalyzer;
    }

    public HashSet<string> GetAllModifiedVariables(Procedure procedure)
    {
        var result = new HashSet<string>();

        foreach (var stmt in procedure.StatementsList)
        {
            result.UnionWith(CollectModifiedVariablesDeep(stmt));
        }

        return result;
    }

    private HashSet<string> CollectModifiedVariablesDeep(Statement stmt)
    {
        var result = new HashSet<string>();

        result.UnionWith(statementAnalyzer.GetModifiedVariables(stmt));

        if (stmt is WhileStatement whileStmt)
        {
            foreach (var inner in whileStmt.StatementsList)
            {
                result.UnionWith(CollectModifiedVariablesDeep(inner)); 
            }
        }

        return result;
    }


    public HashSet<string> GetAllUsedVariables(Procedure procedure)
    {
        var result = new HashSet<string>();

        foreach (var stmt in procedure.StatementsList)
        {
            result.UnionWith(statementAnalyzer.GetUsedVariables(stmt));
        }

        return result;
    }

    public List<string> GetAllAssignmentStatements(Procedure procedure)
    {
        var assignments = new List<string>();

        foreach (var stmt in procedure.StatementsList)
        {
            TraverseAssignments(stmt, assignments);
        }

        return assignments;
    }

    private void TraverseAssignments(Statement stmt, List<string> assignments)
    {
        if (stmt is AssignStatement assign)
        {
            assignments.Add(assign.VariableName + " = ...");
        }
        else if (stmt is WhileStatement whileStmt)
        {
            foreach (var innerStmt in whileStmt.StatementsList)
            {
                TraverseAssignments(innerStmt, assignments); // rekurencja
            }
        }

    }

}
