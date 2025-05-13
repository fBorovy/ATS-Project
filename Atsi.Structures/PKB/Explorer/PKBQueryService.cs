using Atsi.Structures.SIMPLE;
using System.Collections.Generic;
using System.Linq;

namespace Atsi.Structures.PKB.Explorer
{
    public class PKBQueryService : IPKBQuery
    {
        private readonly PKBStorage _db = PKBStorage.Instance;

        // === Procedure ===
        public Procedure? GetProcedure(string name) => _db.GetProcedure(name);

        public IEnumerable<string> GetAllProcedureNames() =>
            _db.GetProcedures().Keys;

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

    }
}
