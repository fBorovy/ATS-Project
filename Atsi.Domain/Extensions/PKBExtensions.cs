using Atsi.Structures.PKB;
using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Statements;
using System.Diagnostics;

namespace Atsi.Domain.Extensions
{
    public static class PKBExtensions
    {
        public static void AddProcedure(Procedure procedure)
        {
            if (!string.IsNullOrEmpty(procedure.Name))
            {
                Statement? previousStmt = null;

                foreach (var statement in procedure.StatementsList)
                {
                    Debug.Assert(statement.StatementNumber != 0);

                    AddStatement(statement);

                    if (previousStmt != null)
                    {
                        AddFollows(previousStmt.StatementNumber, statement.StatementNumber);
                    }

                    previousStmt = statement;
                }

                PKBStorage.Instance.AddProcedure(procedure.Name, procedure);
            }
        }

        public static void AddStatement(Statement statement)
        {
            switch (statement)
            {
                case AssignStatement assignStmt:
                    {
                        AddModifies(assignStmt.StatementNumber, assignStmt.VariableName);

                        foreach (var variable in assignStmt.Expression.GetUsedVariables())
                        {
                            AddUses(assignStmt.StatementNumber, variable);
                        }
                        break;
                    }
                case WhileStatement whileStmt:
                    {
                        AddUses(whileStmt.StatementNumber, whileStmt.ConditionalVariableName);

                        foreach (var stmt in whileStmt.StatementsList)
                        {
                            AddStatement(stmt);
                            AddParent(whileStmt.StatementNumber, stmt.StatementNumber);
                        }
                        break;
                    }
                default: break;
            }
        }

        private static void AddFollows(int stmt1, int stmt2)
        {
            PKBStorage.Instance.AddFollows(stmt1, stmt2);
        }

        private static void AddParent(int parentStmt, int childStmt)
        {
            PKBStorage.Instance.AddParent(parentStmt, childStmt);
        }

        private static void AddModifies(int stmt, string variable)
        {
            PKBStorage.Instance.AddModifies(stmt, variable);
        }

        private static void AddUses(int stmt, string variable)
        {
            PKBStorage.Instance.AddUses(stmt, variable);
        }
    }
}
