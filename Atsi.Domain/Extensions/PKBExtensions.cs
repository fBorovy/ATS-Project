using Atsi.Structures.PKB;
using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Statements;
using Atsi.Structures.Utils.Enums;
using System.Diagnostics;

namespace Atsi.Domain.Extensions
{
    public static class PKBExtensions
    {
        public static Procedure CreateProdecure(string name, List<Statement> statements)
        {
            var proc = new Procedure(name)
            {
                StatementsList = statements
            };
            return proc;
        }

        public static WhileStatement CreateWhileStatement(string conditionalVariableName)
        {
            return new WhileStatement(PKBStorage.GetNextStatementNumber(), conditionalVariableName, []);
        }

        public static WhileStatement UpdateWhileStatement(WhileStatement statement, List<Statement> statements)
        {
            statement.StatementsList = statements;
            return statement;
        }

        //Expression is the right side of assign operation for eg. x = 21 + t the "21 + t" is expression
        public static AssignStatement CreateAssignStatement(string variableName, Expression expression)
        {
            return new AssignStatement(PKBStorage.GetNextStatementNumber(), variableName, expression);
        }

        public static BinaryExpression CreateBinaryExpression(Expression left, DictAvailableArythmeticSymbols _operator, Expression right)
        {
            return new BinaryExpression(left, _operator, right);
        }

        public static ConstExpression CreateConstExpression(int value)
        {
            return new ConstExpression(value);
        }

        public static VariableExpression CreateVariableExpression(string variableName)
        {
            return new VariableExpression(variableName);
        }

        public static void AddProcedureToPKBStorage(Procedure procedure)
        {
            if (!string.IsNullOrEmpty(procedure.Name))
            {
                Statement? previousStmt = null;

                foreach (var statement in procedure.StatementsList)
                {
                    Debug.Assert(statement.StatementNumber != 0);

                    CreatePKBRelationsForStatement(statement);

                    if (previousStmt != null)
                    {
                        AddFollows(previousStmt.StatementNumber, statement.StatementNumber);
                    }

                    previousStmt = statement;
                }

                PKBStorage.Instance.AddProcedure(procedure.Name, procedure);
            }
        }

        public static void CreatePKBRelationsForStatement(Statement statement)
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
                            CreatePKBRelationsForStatement(stmt);
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