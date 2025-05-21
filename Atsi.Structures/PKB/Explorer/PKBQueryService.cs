using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Atsi.Structures.PKB.Explorer
{
    public class PKBQueryService : IPKBQuery
    {
        private readonly PKBStorage _db = PKBStorage.Instance;

        // === Procedure ===
        public Procedure? GetProcedure(string name) => _db.GetProcedure(name);

        public IEnumerable<string> GetAllProcedureNames() =>
            _db.GetProcedures().Keys;

        public IEnumerable<string> GetAllProceduresModifyingAnything()
        {
            return _db.GetProcedureModifies()
                      .Where(kvp => kvp.Value.Any()) // procedura modyfikuje co najmniej jedną zmienną
                      .Select(kvp => kvp.Key);
        }

        public IEnumerable<string> GetAllProceduresUsingAnything()
        {
            return _db.GetProcedureUses()
                      .Where(kvp => kvp.Value.Any())
                      .Select(kvp => kvp.Key);
        }

        public IEnumerable<string> GetAllCallingProcedures()
        {
            return _db.GetCalls()
                      .Where(kvp => kvp.Value.Any()) 
                      .Select(kvp => kvp.Key);
        }

        public IEnumerable<string> GetAllCalledProcedures()
        {
            return _db.GetCalls()
                      .SelectMany(kvp => kvp.Value)
                      .Distinct();
        }

        public IEnumerable<string> GetCallingProceduresT(string callee)
        {
            var result = new HashSet<string>();
            var callsStar = _db.GetCallsStar();

            foreach (var caller in callsStar.Keys)
            {
                if (callsStar[caller].Contains(callee))
                {
                    result.Add(caller);
                }
            }

            return result;
        }

        public IEnumerable<string> GetCalledProceduresT(string caller)
        {
            return _db.GetCallsStar()
                      .TryGetValue(caller, out var callees)
                      ? callees
                      : Enumerable.Empty<string>();
        }

        // === Follows ===
        public bool IsFollows(int stmt1, int stmt2) =>
            _db.IsFollows(stmt1, stmt2);

        public int? GetFollows(int stmt1) =>
            _db.GetFollows().TryGetValue(stmt1, out var next) ? next : null;

        public int? GetFollowedBy(int stmt2) =>
            _db.GetFollows().FirstOrDefault(kvp => kvp.Value == stmt2).Key;

        public IEnumerable<int> GetAllFollowsSources() =>
            _db.GetFollows().Keys;

        public IEnumerable<int> GetAllFollowsTargets() =>
            _db.GetFollows().Values.Distinct();

        public bool IsFollowsTransitive(int stmt1, int stmt2)
        {
            var current = stmt1;
            var follows = _db.GetFollows();
            while (follows.TryGetValue(current, out var next))
            {
                if (next == stmt2) return true;
                current = next;
            }
            return false;
        }

        public IEnumerable<int> GetAllFollowingStatements(int stmt1)
        {
            var result = new List<int>();
            var current = stmt1;
            var follows = _db.GetFollows();
            while (follows.TryGetValue(current, out var next))
            {
                result.Add(next);
                current = next;
            }
            return result;
        }

        public IEnumerable<int> GetAllStatementsLeadingTo(int stmt2)
        {
            var follows = _db.GetFollows();
            return follows.Where(kvp => IsFollowsTransitive(kvp.Key, stmt2)).Select(kvp => kvp.Key);
        }

        // === Parent ===
        public bool IsParent(int parentStmt, int childStmt) =>
            _db.IsParent(parentStmt, childStmt);

        public int? GetParent(int childStmt) =>
            _db.GetParent().TryGetValue(childStmt, out var parent) ? parent : null;

        public IEnumerable<int> GetChildren(int parentStmt) =>
            _db.GetParent().Where(kvp => kvp.Value == parentStmt).Select(kvp => kvp.Key);

        public bool IsNestedIn(int parentStmt, int childStmt)
        {
            var current = childStmt;
            var parents = _db.GetParent();
            while (parents.TryGetValue(current, out var parent))
            {
                if (parent == parentStmt) return true;
                current = parent;
            }
            return false;
        }

        public IEnumerable<int> GetAllNestedStatements(int parentStmt)
        {
            var result = new HashSet<int>();
            var stack = new Stack<int>(GetChildren(parentStmt));
            while (stack.Any())
            {
                var current = stack.Pop();
                if (result.Add(current))
                {
                    foreach (var child in GetChildren(current))
                        stack.Push(child);
                }
            }
            return result;
        }

        public IEnumerable<int> GetAllParentStatements(int childStmt)
        {
            var result = new List<int>();
            var current = childStmt;
            var parents = _db.GetParent();
            while (parents.TryGetValue(current, out var parent))
            {
                result.Add(parent);
                current = parent;
            }
            return result;
        }

        public IEnumerable<(int parent, int child)> GetAllParentPairs()
        {
            return _db.GetParent().Select(kvp => (kvp.Value, kvp.Key));
        }

        public IEnumerable<(int parent, int descendant)> GetAllNestedPairs()
        {
            var parents = _db.GetParent();
            var result = new List<(int, int)>();
            foreach (var parent in parents.Values.Distinct())
            {
                foreach (var descendant in GetAllNestedStatements(parent))
                {
                    result.Add((parent, descendant));
                }
            }
            return result;
        }

        public IEnumerable<int> GetAllParentStatements() =>
            _db.GetParent().Values.Distinct();

        public IEnumerable<int> GetAllChildStatements() =>
            _db.GetParent().Keys.Distinct();

        // === Modifies ===
        public bool IsModifies(int stmt, string variable) =>
            _db.IsModifies(stmt, variable);

        public IEnumerable<string> GetModifiedVariables(int stmt)
        {
            var modifies = _db.GetStatementModifies();
            return modifies.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();
        }

        public IEnumerable<int> GetStatementsModifying(string variable) =>
            _db.GetStatementModifies().Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);

        public IEnumerable<int> GetAllStatementsModifyingAnything() =>
            _db.GetStatementModifies().Keys;

        public IEnumerable<string> GetAllModifiedVariables() =>
            _db.GetStatementModifies().Values.SelectMany(set => set).Distinct();

        // === Uses ===
        public bool IsUses(int stmt, string variable) =>
            _db.IsUses(stmt, variable);

        public IEnumerable<string> GetUsedVariables(int stmt)
        {
            var uses = _db.GetStatementUses();
            return uses.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();
        }

        public IEnumerable<int> GetStatementsUsing(string variable) =>
            _db.GetStatementUses().Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);

        public IEnumerable<int> GetAllStatementsUsingAnything() =>
            _db.GetStatementUses().Keys;

        public IEnumerable<string> GetAllUsedVariables() =>
            _db.GetStatementUses().Values.SelectMany(vars => vars).Distinct();

        // === Calls ===
        public bool IsCalls(string caller, string callee) =>
            _db.IsCalls(caller, callee);

        public bool IsCallsStar(string caller, string callee) =>
            _db.IsCallsStar(caller, callee);

        public IEnumerable<string> GetCalledProcedures(string caller)
        {
            return _db.GetCalls().TryGetValue(caller, out var callees) ? callees : Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetCallingProcedures(string callee)
        {
            return _db.GetCalls().Where(kvp => kvp.Value.Contains(callee)).Select(kvp => kvp.Key);
        }

        public IEnumerable<(string caller, string callee)> GetAllCalls()
        {
            return _db.GetCalls()
                      .SelectMany(kvp => kvp.Value.Select(callee => (kvp.Key, callee)));
        }
       
        public IEnumerable<(string caller, string callee)> GetAllCallsStar()
        {
            return _db.GetCallsStar()
                      .SelectMany(kvp => kvp.Value.Select(callee => (kvp.Key, callee)));
        }

        // === Next ===
        public bool IsNext(int stmt1, int stmt2) =>
            _db.IsNext(stmt1, stmt2);

        public bool IsNextStar(int stmt1, int stmt2) =>
            _db.IsNextStar(stmt1, stmt2);

        public IEnumerable<int> GetNextStatements(int stmt1)
        {
            return _db.GetNext().TryGetValue(stmt1, out var set) ? set : Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetPreviousStatements(int stmt2)
        {
            return _db.GetNext().Where(kvp => kvp.Value.Contains(stmt2)).Select(kvp => kvp.Key);
        }

        // === Affects ===
        public bool IsAffects(int stmt1, int stmt2) =>
            _db.IsAffects(stmt1, stmt2);

        public bool IsAffectsStar(int stmt1, int stmt2) =>
            _db.IsAffectsStar(stmt1, stmt2);

        public IEnumerable<int> GetAffectedStatements(int stmt1)
        {
            return _db.GetAffects().TryGetValue(stmt1, out var set) ? set : Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetAffectingStatements(int stmt2)
        {
            return _db.GetAffects().Where(kvp => kvp.Value.Contains(stmt2)).Select(kvp => kvp.Key);
        }

        // === Procedure-Level Modifies / Uses ===
        public bool IsModifies(string procedure, string variable) =>
            _db.IsModifies(procedure, variable);

        public IEnumerable<string> GetModifiedVariables(string procedure)
        {
            return _db.GetProcedureModifies().TryGetValue(procedure, out var vars) ? vars : Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetProceduresModifying(string variable)
        {
            return _db.GetProcedureModifies().Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);
        }

        public bool IsUses(string procedure, string variable) =>
            _db.IsUses(procedure, variable);

        public IEnumerable<string> GetUsedVariables(string procedure)
        {
            return _db.GetProcedureUses().TryGetValue(procedure, out var vars) ? vars : Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetProceduresUsing(string variable)
        {
            return _db.GetProcedureUses().Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);
        }

        private IEnumerable<int> GetAllStatementsOfType<T>() where T : Statement
        {
            var result = new List<int>();

            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is T typedStmt)
                        result.Add(typedStmt.StatementNumber);
                }
            }

            return result;
        }

        private IEnumerable<Statement> GetAllDescendantStatements(IEnumerable<Statement> statements)
        {
            foreach (var stmt in statements)
            {
                yield return stmt;

                if (stmt is IfStatement ifStmt)
                {
                    foreach (var inner in GetAllDescendantStatements(ifStmt.ThenBodyStatements))
                        yield return inner;

                    foreach (var inner in GetAllDescendantStatements(ifStmt.ElseBodyStatements))
                        yield return inner;
                }
                else if (stmt is WhileStatement whileStmt)
                {
                    foreach (var inner in GetAllDescendantStatements(whileStmt.StatementsList))
                        yield return inner;
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////// Get all Ifs, Assigns, Whiles ////////////////////////////////////////////////////////////////////

        public IEnumerable<int> GetAllWhiles()
        {
            return GetAllStatementsOfType<WhileStatement>();
        }

        public IEnumerable<int> GetAllIfs()
        {
            return GetAllStatementsOfType<IfStatement>();
        }

        public IEnumerable<int> GetAllAssigns()
        {
            return GetAllStatementsOfType<AssignStatement>();
        }

        public IEnumerable<int> GetAllCallsNumbers()
        {
            return GetAllStatementsOfType<CallStatement>();
        }

        public IEnumerable<int> GetAllStatements()
        {
            return GetAllStatementsOfType<Statement>();
        }

        public IEnumerable<int> GetAssignmentsUsing(string variable)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is AssignStatement assignStmt &&
                        assignStmt.Expression.GetUsedVariables().Contains(variable))
                    {
                        yield return assignStmt.StatementNumber;
                    }
                }
            }
        }

        public IEnumerable<int> GetIfsUsing(string variable)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is IfStatement ifStmt && ifStmt.ConditionalVariableName == variable)
                    {
                        yield return ifStmt.StatementNumber;
                    }
                }
            }
        }

        public IEnumerable<int> GetWhilesUsing(string variable)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is WhileStatement whileStmt && whileStmt.ConditionalVariableName == variable)
                    {
                        yield return whileStmt.StatementNumber;
                    }
                }
            }
        }

        public IEnumerable<int> GetAllFollowedWhiles()
        {
            var follows = _db.GetFollows(); 
            var allWhiles = GetAllWhiles(); 
            return allWhiles.Where(stmt => follows.ContainsKey(stmt));
        }

        public IEnumerable<int> GetAllAssignmentsUsingAnything()
        {
            var uses = _db.GetStatementUses();
            var assigns = GetAllAssigns(); 
            return assigns.Where(stmt => uses.ContainsKey(stmt) && uses[stmt].Any());
        }

        public IEnumerable<int> GetAllWhilesUsingAnything()
        {
            var uses = _db.GetStatementUses();
            var whiles = GetAllWhiles();
            return whiles.Where(stmt => uses.ContainsKey(stmt) && uses[stmt].Any());
        }

        public IEnumerable<int> GetAllIfsUsingAnything()
        {
            var uses = _db.GetStatementUses();
            var ifs = GetAllIfs();
            return ifs.Where(stmt => uses.ContainsKey(stmt) && uses[stmt].Any());
        }

        public IEnumerable<int> GetAllParentIfs()
        {
            var allIfs = GetAllIfs();
            var parentKeys = _db.GetParent().Values.Distinct();
            return allIfs.Intersect(parentKeys);
        }

        public IEnumerable<int> GetAllParentWhiles()
        {
            var allWhiles = GetAllWhiles();
            var parentKeys = _db.GetParent().Values.Distinct();
            return allWhiles.Intersect(parentKeys);
        }

        public IEnumerable<string> GetUsedVariablesByAssign(int stmtNumber)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is AssignStatement assignStmt && assignStmt.StatementNumber == stmtNumber)
                    {
                        return assignStmt.Expression.GetUsedVariables();
                    }
                }
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetUsedVariablesByWhile(int stmtNumber)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is WhileStatement whileStmt && whileStmt.StatementNumber == stmtNumber)
                    {
                        return new[] { whileStmt.ConditionalVariableName };
                    }
                }
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetUsedVariablesByIf(int stmtNumber)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is IfStatement ifStmt && ifStmt.StatementNumber == stmtNumber)
                    {
                        return new[] { ifStmt.ConditionalVariableName };
                    }
                }
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<int> GetChildrenOfIf(int ifStmt)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is IfStatement ifNode && ifNode.StatementNumber == ifStmt)
                    {
                        return ifNode.ThenBodyStatements.Concat(ifNode.ElseBodyStatements)
                                                        .Select(s => s.StatementNumber);
                    }
                }
            }

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetChildrenOfWhile(int whileStmt)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is WhileStatement whileNode && whileNode.StatementNumber == whileStmt)
                    {
                        return whileNode.StatementsList.Select(s => s.StatementNumber);
                    }
                }
            }

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetAllNestedStatementsInIfT(int ifStmt)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is IfStatement ifNode && ifNode.StatementNumber == ifStmt)
                    {
                        return GetAllDescendantStatements(ifNode.ThenBodyStatements.Concat(ifNode.ElseBodyStatements))
                               .Select(s => s.StatementNumber);
                    }
                }
            }

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetAllNestedStatementsInWhileT(int whileStmt)
        {
            foreach (var procedure in _db.GetProcedures().Values)
            {
                foreach (var stmt in GetAllDescendantStatements(procedure.StatementsList))
                {
                    if (stmt is WhileStatement whileNode && whileNode.StatementNumber == whileStmt)
                    {
                        return GetAllDescendantStatements(whileNode.StatementsList)
                               .Select(s => s.StatementNumber);
                    }
                }
            }

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetAllChildStatementsOfIfs()
        {
            return GetAllIfs()
                   .SelectMany(GetChildrenOfIf)
                   .Distinct();
        }

        public IEnumerable<int> GetAllChildStatementsOfWhiles()
        {
            return GetAllWhiles()
                   .SelectMany(GetChildrenOfWhile)
                   .Distinct();
        }

        public IEnumerable<int> GetAllChildStatementsOfIfsT()
        {
            return GetAllIfs()
                   .SelectMany(GetAllNestedStatementsInIfT)
                   .Distinct();
        }

        public IEnumerable<int> GetAllChildStatementsOfWhilesT()
        {
            return GetAllWhiles()
                   .SelectMany(GetAllNestedStatementsInWhileT)
                   .Distinct();
        }

        public int? GetFollowedByIf(int followingStmtNumber)
        {
            var follows = _db.GetFollows();
            var ifs = GetAllIfs();
            return follows.FirstOrDefault(kvp => kvp.Value == followingStmtNumber && ifs.Contains(kvp.Key)).Key;
        }

        public int? GetFollowedByWhile(int followingStmtNumber)
        {
            var follows = _db.GetFollows();
            var whiles = GetAllWhiles();
            return follows.FirstOrDefault(kvp => kvp.Value == followingStmtNumber && whiles.Contains(kvp.Key)).Key;
        }

        public int? GetFollowedByCall(int followingStmtNumber)
        {
            var follows = _db.GetFollows();
            var calls = GetAllCallsNumbers();
            return follows.FirstOrDefault(kvp => kvp.Value == followingStmtNumber && calls.Contains(kvp.Key)).Key;
        }

        public int? GetFollowedByAssign(int followingStmtNumber)
        {
            var follows = _db.GetFollows();
            var assigns = GetAllAssigns();
            return follows.FirstOrDefault(kvp => kvp.Value == followingStmtNumber && assigns.Contains(kvp.Key)).Key;
        }
    }
}
