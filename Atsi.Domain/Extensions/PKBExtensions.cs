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
        public static Procedure CreateProcedure(string name, List<Statement> statements)
        {
            return new Procedure(name)
            {
                StatementsList = statements
            };
        }

        public static WhileStatement CreateWhileStatement(string procedureName, string conditionalVariableName)
        {
            return new WhileStatement
            {
                ProcedureName = procedureName,
                ConditionalVariableName = conditionalVariableName,
                StatementNumber = PKBStorage.GetNextStatementNumber()
            };
        }

        public static WhileStatement UpdateWhileStatement(WhileStatement statement, List<Statement> statements)
        {
            statement.StatementsList = statements;
            return statement;
        }

        public static IfStatement CreateIfStatement(string procedureName, string conditionalVariableName)
        {
            return new IfStatement
            {
                ProcedureName = procedureName,
                ConditionalVariableName = conditionalVariableName,
                StatementNumber = PKBStorage.GetNextStatementNumber()
            };
        }

        public static IfStatement UpdateIfStatement(IfStatement statement, List<Statement> thenStmts, List<Statement> elseStmts)
        {
            statement.ThenBodyStatements = thenStmts;
            statement.ElseBodyStatements = elseStmts;
            return statement;
        }
        public static CallStatement CreateCallStatement(string procedureName, string calledProcedureName)
        {
            return new CallStatement
            {
                ProcedureName = procedureName,
                CalledProcedureName = calledProcedureName,
                StatementNumber = PKBStorage.GetNextStatementNumber()
            };
        }

        public static AssignStatement CreateAssignStatement(string procedureName, string variableName, Expression expression)
        {
            return new AssignStatement()
            {
                ProcedureName = procedureName,
                VariableName = variableName,
                Expression = expression,
                StatementNumber = PKBStorage.GetNextStatementNumber()
            };
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
            if (string.IsNullOrEmpty(procedure.Name)) return;

            Statement? previousStmt = null;

            var procModifies = new HashSet<string>();
            var procUses = new HashSet<string>();

            foreach (var statement in procedure.StatementsList)
            {
                Debug.Assert(statement.StatementNumber != 0);

                CreatePKBRelationsForStatement(statement, ref procModifies, ref procUses);

                if (previousStmt != null)
                {
                    AddFollows(previousStmt.StatementNumber, statement.StatementNumber);
                }

                previousStmt = statement;
            }

            foreach (var mod in procModifies)
                PKBStorage.Instance.AddModifies(procedure.Name, mod);

            foreach (var use in procUses)
                PKBStorage.Instance.AddUses(procedure.Name, use);

            PKBStorage.Instance.AddProcedure(procedure.Name, procedure);
        }

        public static void CreatePKBRelationsForStatement(Statement statement, ref HashSet<string> procModifies, ref HashSet<string> procUses)
        {
            switch (statement)
            {
                case AssignStatement assignStmt:
                    AddModifies(assignStmt.StatementNumber, assignStmt.VariableName);
                    procModifies.Add(assignStmt.VariableName);

                    foreach (var variable in assignStmt.Expression.GetUsedVariables())
                    {
                        AddUses(assignStmt.StatementNumber, variable);
                        procUses.Add(variable);
                    }
                    break;

                case WhileStatement whileStmt:
                    AddUses(whileStmt.StatementNumber, whileStmt.ConditionalVariableName);
                    procUses.Add(whileStmt.ConditionalVariableName);

                    Statement? prev = null;
                    foreach (var stmt in whileStmt.StatementsList)
                    {
                        CreatePKBRelationsForStatement(stmt, ref procModifies, ref procUses);
                        AddParent(whileStmt.StatementNumber, stmt.StatementNumber);

                        if (prev != null)
                            AddFollows(prev.StatementNumber, stmt.StatementNumber);

                        prev = stmt;
                    }
                    break;

                case IfStatement ifStmt:
                    AddUses(ifStmt.StatementNumber, ifStmt.ConditionalVariableName);
                    procUses.Add(ifStmt.ConditionalVariableName);

                    Statement? prevThen = null;
                    foreach (var stmt in ifStmt.ThenBodyStatements)
                    {
                        CreatePKBRelationsForStatement(stmt, ref procModifies, ref procUses);
                        AddParent(ifStmt.StatementNumber, stmt.StatementNumber);

                        if (prevThen != null)
                            AddFollows(prevThen.StatementNumber, stmt.StatementNumber);

                        prevThen = stmt;
                    }

                    Statement? prevElse = null;
                    foreach (var stmt in ifStmt.ElseBodyStatements)
                    {
                        CreatePKBRelationsForStatement(stmt, ref procModifies, ref procUses);
                        AddParent(ifStmt.StatementNumber, stmt.StatementNumber);

                        if (prevElse != null)
                            AddFollows(prevElse.StatementNumber, stmt.StatementNumber);

                        prevElse = stmt;
                    }
                    break;

                case CallStatement callStmt:
                    AddCalls(statement.ProcedureName, callStmt.CalledProcedureName);
                    break;
            }
        }

        // --- Relationship Helpers ---
        private static void AddFollows(int stmt1, int stmt2) => PKBStorage.Instance.AddFollows(stmt1, stmt2);
        private static void AddParent(int parentStmt, int childStmt) => PKBStorage.Instance.AddParent(parentStmt, childStmt);
        private static void AddModifies(int stmt, string variable) => PKBStorage.Instance.AddModifies(stmt, variable);
        private static void AddUses(int stmt, string variable) => PKBStorage.Instance.AddUses(stmt, variable);
        private static void AddCalls(string caller, string callee) => PKBStorage.Instance.AddCalls(caller, callee);
    }

}