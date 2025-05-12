using Atsi.Structures.PKB;
using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Statements;
using Atsi.Structures.Utils.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Atsi.Domain.Extensions.Tests
{
    [TestClass]
    public class PKBExtensionsTests
    {
        private PKBStorage _pkbStorage;

        [TestInitialize]
        public void TestInitialize()
        {
            _pkbStorage = PKBStorage.Instance;
            _pkbStorage.Clear(); 
        }

        #region CreateProcedure Tests
        [TestMethod]
        public void CreateProcedure_ValidNameAndStatements_ReturnsProcedure()
        {
            // Arrange
            var name = "testProc";
            var statements = new List<Statement>();

            // Act
            var result = PKBExtensions.CreateProcedure(name, statements);

            // Assert
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(statements, result.StatementsList);
        }

        [TestMethod]
        public void CreateProcedure_EmptyName_ReturnsProcedure()
        {
            // Arrange
            var name = "";
            var statements = new List<Statement>();

            // Act
            var result = PKBExtensions.CreateProcedure(name, statements);

            // Assert
            Assert.AreEqual(name, result.Name);
        }

        [TestMethod]
        public void CreateProcedure_NullStatements_ReturnsProcedure()
        {
            // Arrange
            var name = "testProc";
            List<Statement> statements = null;

            // Act
            var result = PKBExtensions.CreateProcedure(name, statements);

            // Assert
            Assert.AreEqual(name, result.Name);
            Assert.IsNull(result.StatementsList);
        }
        #endregion

        #region CreateWhileStatement Tests
        [TestMethod]
        public void CreateWhileStatement_ValidParameters_ReturnsWhileStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "x";

            // Act
            var result = PKBExtensions.CreateWhileStatement(procName, varName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
            Assert.AreEqual(varName, result.ConditionalVariableName);
            Assert.IsTrue(result.StatementNumber > 0);
        }

        [TestMethod]
        public void CreateWhileStatement_EmptyProcedureName_ReturnsWhileStatement()
        {
            // Arrange
            var procName = "";
            var varName = "x";

            // Act
            var result = PKBExtensions.CreateWhileStatement(procName, varName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
        }

        [TestMethod]
        public void CreateWhileStatement_EmptyVariableName_ReturnsWhileStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "";

            // Act
            var result = PKBExtensions.CreateWhileStatement(procName, varName);

            // Assert
            Assert.AreEqual(varName, result.ConditionalVariableName);
        }
        #endregion

        #region UpdateWhileStatement Tests
        [TestMethod]
        public void UpdateWhileStatement_WithStatements_UpdatesCorrectly()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "x");
            var statements = new List<Statement>
            {
                PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(1))
            };

            // Act
            var result = PKBExtensions.UpdateWhileStatement(whileStmt, statements);

            // Assert
            Assert.AreEqual(statements, result.StatementsList);
        }

        [TestMethod]
        public void UpdateWhileStatement_EmptyStatements_UpdatesCorrectly()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "x");
            var statements = new List<Statement>();

            // Act
            var result = PKBExtensions.UpdateWhileStatement(whileStmt, statements);

            // Assert
            Assert.AreEqual(0, result.StatementsList.Count);
        }

        [TestMethod]
        public void UpdateWhileStatement_NullStatements_UpdatesCorrectly()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "x");
            List<Statement> statements = null;

            // Act
            var result = PKBExtensions.UpdateWhileStatement(whileStmt, statements);

            // Assert
            Assert.IsNull(result.StatementsList);
        }
        #endregion

        #region CreateIfStatement Tests
        [TestMethod]
        public void CreateIfStatement_ValidParameters_ReturnsIfStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "x";

            // Act
            var result = PKBExtensions.CreateIfStatement(procName, varName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
            Assert.AreEqual(varName, result.ConditionalVariableName);
            Assert.IsTrue(result.StatementNumber > 0);
        }

        [TestMethod]
        public void CreateIfStatement_EmptyProcedureName_ReturnsIfStatement()
        {
            // Arrange
            var procName = "";
            var varName = "x";

            // Act
            var result = PKBExtensions.CreateIfStatement(procName, varName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
        }

        [TestMethod]
        public void CreateIfStatement_EmptyVariableName_ReturnsIfStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "";

            // Act
            var result = PKBExtensions.CreateIfStatement(procName, varName);

            // Assert
            Assert.AreEqual(varName, result.ConditionalVariableName);
        }
        #endregion

        #region UpdateIfStatement Tests
        [TestMethod]
        public void UpdateIfStatement_WithThenAndElse_UpdatesCorrectly()
        {
            // Arrange
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "x");
            var thenStmts = new List<Statement>
            {
                PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(1))
            };
            var elseStmts = new List<Statement>
            {
                PKBExtensions.CreateAssignStatement("proc1", "z", PKBExtensions.CreateConstExpression(2))
            };

            // Act
            var result = PKBExtensions.UpdateIfStatement(ifStmt, thenStmts, elseStmts);

            // Assert
            Assert.AreEqual(thenStmts, result.ThenBodyStatements);
            Assert.AreEqual(elseStmts, result.ElseBodyStatements);
        }

        [TestMethod]
        public void UpdateIfStatement_EmptyThen_UpdatesCorrectly()
        {
            // Arrange
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "x");
            var thenStmts = new List<Statement>();
            var elseStmts = new List<Statement>
            {
                PKBExtensions.CreateAssignStatement("proc1", "z", PKBExtensions.CreateConstExpression(2))
            };

            // Act
            var result = PKBExtensions.UpdateIfStatement(ifStmt, thenStmts, elseStmts);

            // Assert
            Assert.AreEqual(0, result.ThenBodyStatements.Count);
        }

        [TestMethod]
        public void UpdateIfStatement_NullElse_UpdatesCorrectly()
        {
            // Arrange
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "x");
            var thenStmts = new List<Statement>
            {
                PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(1))
            };
            List<Statement> elseStmts = null;

            // Act
            var result = PKBExtensions.UpdateIfStatement(ifStmt, thenStmts, elseStmts);

            // Assert
            Assert.IsNull(result.ElseBodyStatements);
        }
        #endregion

        #region CreateCallStatement Tests
        [TestMethod]
        public void CreateCallStatement_ValidParameters_ReturnsCallStatement()
        {
            // Arrange
            var procName = "proc1";
            var calledProcName = "proc2";

            // Act
            var result = PKBExtensions.CreateCallStatement(procName, calledProcName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
            Assert.AreEqual(calledProcName, result.CalledProcedureName);
            Assert.IsTrue(result.StatementNumber > 0);
        }

        [TestMethod]
        public void CreateCallStatement_EmptyProcedureName_ReturnsCallStatement()
        {
            // Arrange
            var procName = "";
            var calledProcName = "proc2";

            // Act
            var result = PKBExtensions.CreateCallStatement(procName, calledProcName);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
        }

        [TestMethod]
        public void CreateCallStatement_EmptyCalledProcedureName_ReturnsCallStatement()
        {
            // Arrange
            var procName = "proc1";
            var calledProcName = "";

            // Act
            var result = PKBExtensions.CreateCallStatement(procName, calledProcName);

            // Assert
            Assert.AreEqual(calledProcName, result.CalledProcedureName);
        }
        #endregion

        #region CreateAssignStatement Tests
        [TestMethod]
        public void CreateAssignStatement_ValidParameters_ReturnsAssignStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "x";
            var expr = PKBExtensions.CreateConstExpression(5);

            // Act
            var result = PKBExtensions.CreateAssignStatement(procName, varName, expr);

            // Assert
            Assert.AreEqual(procName, result.ProcedureName);
            Assert.AreEqual(varName, result.VariableName);
            Assert.AreEqual(expr, result.Expression);
            Assert.IsTrue(result.StatementNumber > 0);
        }

        [TestMethod]
        public void CreateAssignStatement_EmptyVariableName_ReturnsAssignStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "";
            var expr = PKBExtensions.CreateConstExpression(5);

            // Act
            var result = PKBExtensions.CreateAssignStatement(procName, varName, expr);

            // Assert
            Assert.AreEqual(varName, result.VariableName);
        }

        [TestMethod]
        public void CreateAssignStatement_NullExpression_ReturnsAssignStatement()
        {
            // Arrange
            var procName = "proc1";
            var varName = "x";
            Expression expr = null;

            // Act
            var result = PKBExtensions.CreateAssignStatement(procName, varName, expr);

            // Assert
            Assert.IsNull(result.Expression);
        }
        #endregion

        #region Expression Creation Tests
        [TestMethod]
        public void CreateBinaryExpression_ValidParameters_ReturnsBinaryExpression()
        {
            // Arrange
            var left = PKBExtensions.CreateConstExpression(1);
            var op = DictAvailableArythmeticSymbols.Plus;
            var right = PKBExtensions.CreateConstExpression(2);

            // Act
            var result = PKBExtensions.CreateBinaryExpression(left, op, right);

            // Assert
            Assert.AreEqual(left, result.LeftExpression);
            Assert.AreEqual(op, result.Operator);
            Assert.AreEqual(right, result.RightExpression);
        }

        [TestMethod]
        public void CreateBinaryExpression_NullLeft_ReturnsBinaryExpression()
        {
            // Arrange
            Expression left = null;
            var op = DictAvailableArythmeticSymbols.Plus;
            var right = PKBExtensions.CreateConstExpression(2);

            // Act
            var result = PKBExtensions.CreateBinaryExpression(left, op, right);

            // Assert
            Assert.IsNull(result.LeftExpression);
        }

        [TestMethod]
        public void CreateConstExpression_ValidValue_ReturnsConstExpression()
        {
            // Arrange
            var value = 42;

            // Act
            var result = PKBExtensions.CreateConstExpression(value);

            // Assert
            Assert.AreEqual(value, result.Value);
        }

        [TestMethod]
        public void CreateVariableExpression_ValidName_ReturnsVariableExpression()
        {
            // Arrange
            var varName = "x";

            // Act
            var result = PKBExtensions.CreateVariableExpression(varName);

            // Assert
            Assert.AreEqual(varName, result.VariableName);
        }

        [TestMethod]
        public void CreateVariableExpression_EmptyName_ReturnsVariableExpression()
        {
            // Arrange
            var varName = "";

            // Act
            var result = PKBExtensions.CreateVariableExpression(varName);

            // Assert
            Assert.AreEqual(varName, result.VariableName);
        }
        #endregion

        #region AddProcedureToPKBStorage Tests
        [TestMethod]
        public void AddProcedureToPKBStorage_ValidProcedure_AddsToStorage()
        {
            // Arrange
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement>());

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.ContainsProcedure("proc1"));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_ProcedureWithAssignStatement_AddsModifiesAndUses()
        {
            // Arrange
            var assign = PKBExtensions.CreateAssignStatement("proc1", "x", 
                PKBExtensions.CreateBinaryExpression(
                    PKBExtensions.CreateVariableExpression("y"),
                    DictAvailableArythmeticSymbols.Plus,
                    PKBExtensions.CreateConstExpression(1)
                ));
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { assign });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Modifies(assign.StatementNumber, "x"));
            Assert.IsTrue(_pkbStorage.Uses(assign.StatementNumber, "y"));
            Assert.IsTrue(_pkbStorage.Modifies("proc1", "x"));
            Assert.IsTrue(_pkbStorage.Uses("proc1", "y"));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_ProcedureWithWhileStatement_AddsParentAndUses()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "cond");
            var assign = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            PKBExtensions.UpdateWhileStatement(whileStmt, new List<Statement> { assign });
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { whileStmt });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Uses(whileStmt.StatementNumber, "cond"));
            Assert.IsTrue(_pkbStorage.Parent(whileStmt.StatementNumber, assign.StatementNumber));
            Assert.IsTrue(_pkbStorage.Uses("proc1", "cond"));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_ProcedureWithIfStatement_AddsParentAndUses()
        {
            // Arrange
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "cond");
            var thenAssign = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            var elseAssign = PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(2));
            PKBExtensions.UpdateIfStatement(ifStmt, new List<Statement> { thenAssign }, new List<Statement> { elseAssign });
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { ifStmt });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Uses(ifStmt.StatementNumber, "cond"));
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, thenAssign.StatementNumber));
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, elseAssign.StatementNumber));
            Assert.IsTrue(_pkbStorage.Uses("proc1", "cond"));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_ProcedureWithCallStatement_AddsCalls()
        {
            // Arrange
            var callStmt = PKBExtensions.CreateCallStatement("proc1", "proc2");
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { callStmt });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Calls("proc1", "proc2"));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_EmptyProcedureName_DoesNotAdd()
        {
            // Arrange
            var proc = PKBExtensions.CreateProcedure("", new List<Statement>());

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsFalse(_pkbStorage.ContainsProcedure(""));
        }

        [TestMethod]
        public void AddProcedureToPKBStorage_ProcedureWithMultipleStatements_AddsFollows()
        {
            // Arrange
            var assign1 = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            var assign2 = PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(2));
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { assign1, assign2 });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Follows(assign1.StatementNumber, assign2.StatementNumber));
        }
        #endregion

        #region CreatePKBRelationsForStatement Tests
        [TestMethod]
        public void CreatePKBRelationsForStatement_AssignStatement_AddsModifiesAndUses()
        {
            // Arrange
            var assign = PKBExtensions.CreateAssignStatement("proc1", "x", 
                PKBExtensions.CreateBinaryExpression(
                    PKBExtensions.CreateVariableExpression("y"),
                    DictAvailableArythmeticSymbols.Plus,
                    PKBExtensions.CreateConstExpression(1)
                ));
            var procModifies = new HashSet<string>();
            var procUses = new HashSet<string>();

            // Act
            PKBExtensions.CreatePKBRelationsForStatement(assign, ref procModifies, ref procUses);

            // Assert
            Assert.IsTrue(_pkbStorage.Modifies(assign.StatementNumber, "x"));
            Assert.IsTrue(_pkbStorage.Uses(assign.StatementNumber, "y"));
            Assert.IsTrue(procModifies.Contains("x"));
            Assert.IsTrue(procUses.Contains("y"));
        }

        [TestMethod]
        public void CreatePKBRelationsForStatement_WhileStatement_AddsParentAndUses()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "cond");
            var assign = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            PKBExtensions.UpdateWhileStatement(whileStmt, new List<Statement> { assign });
            var procModifies = new HashSet<string>();
            var procUses = new HashSet<string>();

            // Act
            PKBExtensions.CreatePKBRelationsForStatement(whileStmt, ref procModifies, ref procUses);

            // Assert
            Assert.IsTrue(_pkbStorage.Uses(whileStmt.StatementNumber, "cond"));
            Assert.IsTrue(_pkbStorage.Parent(whileStmt.StatementNumber, assign.StatementNumber));
            Assert.IsTrue(procUses.Contains("cond"));
        }

        [TestMethod]
        public void CreatePKBRelationsForStatement_IfStatement_AddsParentAndUses()
        {
            // Arrange
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "cond");
            var thenAssign = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            var elseAssign = PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(2));
            PKBExtensions.UpdateIfStatement(ifStmt, new List<Statement> { thenAssign }, new List<Statement> { elseAssign });
            var procModifies = new HashSet<string>();
            var procUses = new HashSet<string>();

            // Act
            PKBExtensions.CreatePKBRelationsForStatement(ifStmt, ref procModifies, ref procUses);

            // Assert
            Assert.IsTrue(_pkbStorage.Uses(ifStmt.StatementNumber, "cond"));
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, thenAssign.StatementNumber));
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, elseAssign.StatementNumber));
            Assert.IsTrue(procUses.Contains("cond"));
        }

        [TestMethod]
        public void CreatePKBRelationsForStatement_CallStatement_AddsCalls()
        {
            // Arrange
            var callStmt = PKBExtensions.CreateCallStatement("proc1", "proc2");
            var procModifies = new HashSet<string>();
            var procUses = new HashSet<string>();

            // Act
            PKBExtensions.CreatePKBRelationsForStatement(callStmt, ref procModifies, ref procUses);

            // Assert
            Assert.IsTrue(_pkbStorage.Calls("proc1", "proc2"));
        }
        #endregion

        #region Relationship Helpers Tests
        [TestMethod]
        public void AddFollows_ValidStatementNumbers_AddsRelationship()
        {
            // Arrange
            var stmt1 = 1;
            var stmt2 = 2;

            // Act
            PKBExtensions.AddFollows(stmt1, stmt2);

            // Assert
            Assert.IsTrue(_pkbStorage.Follows(stmt1, stmt2));
        }

        [TestMethod]
        public void AddParent_ValidStatementNumbers_AddsRelationship()
        {
            // Arrange
            var parent = 1;
            var child = 2;

            // Act
            PKBExtensions.AddParent(parent, child);

            // Assert
            Assert.IsTrue(_pkbStorage.Parent(parent, child));
        }

        [TestMethod]
        public void AddModifies_ValidStatementAndVariable_AddsRelationship()
        {
            // Arrange
            var stmt = 1;
            var var = "x";

            // Act
            PKBExtensions.AddModifies(stmt, var);

            // Assert
            Assert.IsTrue(_pkbStorage.Modifies(stmt, var));
        }

        [TestMethod]
        public void AddUses_ValidStatementAndVariable_AddsRelationship()
        {
            // Arrange
            var stmt = 1;
            var var = "x";

            // Act
            PKBExtensions.AddUses(stmt, var);

            // Assert
            Assert.IsTrue(_pkbStorage.Uses(stmt, var));
        }

        [TestMethod]
        public void AddCalls_ValidProcedures_AddsRelationship()
        {
            // Arrange
            var caller = "proc1";
            var callee = "proc2";

            // Act
            PKBExtensions.AddCalls(caller, callee);

            // Assert
            Assert.IsTrue(_pkbStorage.Calls(caller, callee));
        }
        #endregion

        [TestMethod]
        public void CreateProcedure_WithNestedStatements_CreatesCorrectRelationships()
        {
            // Arrange
            var whileStmt = PKBExtensions.CreateWhileStatement("proc1", "cond");
            var assign1 = PKBExtensions.CreateAssignStatement("proc1", "x", PKBExtensions.CreateConstExpression(1));
            var assign2 = PKBExtensions.CreateAssignStatement("proc1", "y", PKBExtensions.CreateConstExpression(2));
            PKBExtensions.UpdateWhileStatement(whileStmt, new List<Statement> { assign1, assign2 });
            
            var ifStmt = PKBExtensions.CreateIfStatement("proc1", "cond2");
            var thenAssign = PKBExtensions.CreateAssignStatement("proc1", "a", PKBExtensions.CreateConstExpression(3));
            var elseAssign = PKBExtensions.CreateAssignStatement("proc1", "b", PKBExtensions.CreateConstExpression(4));
            PKBExtensions.UpdateIfStatement(ifStmt, new List<Statement> { thenAssign }, new List<Statement> { elseAssign });
            
            var proc = PKBExtensions.CreateProcedure("proc1", new List<Statement> { whileStmt, ifStmt });

            // Act
            PKBExtensions.AddProcedureToPKBStorage(proc);

            // Assert
            Assert.IsTrue(_pkbStorage.Parent(whileStmt.StatementNumber, assign1.StatementNumber));
            Assert.IsTrue(_pkbStorage.Parent(whileStmt.StatementNumber, assign2.StatementNumber));
            Assert.IsTrue(_pkbStorage.Follows(assign1.StatementNumber, assign2.StatementNumber));
            
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, thenAssign.StatementNumber));
            Assert.IsTrue(_pkbStorage.Parent(ifStmt.StatementNumber, elseAssign.StatementNumber));
            
            Assert.IsTrue(_pkbStorage.Follows(whileStmt.StatementNumber, ifStmt.StatementNumber));
            
            Assert.IsTrue(_pkbStorage.Modifies("proc1", "x"));
            Assert.IsTrue(_pkbStorage.Modifies("proc1", "y"));
            Assert.IsTrue(_pkbStorage.Modifies("proc1", "a"));
            Assert.IsTrue(_pkbStorage.Modifies("proc1", "b"));
            Assert.IsTrue(_pkbStorage.Uses("proc1", "cond"));
            Assert.IsTrue(_pkbStorage.Uses("proc1", "cond2"));
        }
    }
}
