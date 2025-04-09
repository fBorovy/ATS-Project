using Atsi.Structures.SIMPLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atsi.Structures.PKB.Explorer
{
    public class PKBQueryService : IPKBQuery
    {
        private readonly PKBStorage _db = PKBStorage.Instance;

        // === Procedure ===
        public Procedure? GetProcedure(string name) => _db.GetProcedure(name);

        public IEnumerable<string> GetAllProcedureNames() =>
            _db.Procedures.Keys;

        // === Follows ===
        public bool IsFollows(int stmt1, int stmt2) =>
            _db.IsFollows(stmt1, stmt2);

        public int? GetFollows(int stmt1) =>
            _db.Follows.TryGetValue(stmt1, out var next) ? next : null;

        public int? GetFollowedBy(int stmt2) =>
            _db.Follows.FirstOrDefault(kvp => kvp.Value == stmt2).Key;
        public IEnumerable<int> GetAllFollowsSources() =>
            _db.Follows.Keys;

        public IEnumerable<int> GetAllFollowsTargets() =>
            _db.Follows.Values.Distinct();

        public bool IsFollowsTransitive(int stmt1, int stmt2)
        {
            var current = stmt1;
            while (_db.Follows.TryGetValue(current, out var next))
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

            while (_db.Follows.TryGetValue(current, out var next))
            {
                result.Add(next);
                current = next;
            }

            return result;
        }

        public IEnumerable<int> GetAllStatementsLeadingTo(int stmt2)
        {
            var result = new List<int>();
            foreach (var (s1, s2) in _db.Follows)
            {
                if (IsFollowsTransitive(s1, stmt2))
                    result.Add(s1);
            }
            return result;
        }


        // === Parent ===
        public bool IsParent(int parentStmt, int childStmt) =>
            _db.IsParent(parentStmt, childStmt);

        public int? GetParent(int childStmt) =>
            _db.Parent.TryGetValue(childStmt, out var parent) ? parent : null;

        public IEnumerable<int> GetChildren(int parentStmt) =>
            _db.Parent.Where(kvp => kvp.Value == parentStmt).Select(kvp => kvp.Key);

        public bool IsNestedIn(int parentStmt, int childStmt)
        {
            var current = childStmt;
            while (_db.Parent.TryGetValue(current, out var parent))
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

            while (_db.Parent.TryGetValue(current, out var parent))
            {
                result.Add(parent);
                current = parent;
            }

            return result;
        }

        public IEnumerable<(int parent, int child)> GetAllParentPairs()
        {
            return _db.Parent.Select(kvp => (kvp.Value, kvp.Key));
        }
        public IEnumerable<(int parent, int descendant)> GetAllNestedPairs()
        {
            var result = new List<(int, int)>();

            foreach (var parent in _db.Parent.Values.Distinct())
            {
                foreach (var descendant in GetAllNestedStatements(parent))
                {
                    result.Add((parent, descendant));
                }
            }

            return result;
        }

        public IEnumerable<int> GetAllParentStatements()
        {
            return _db.Parent.Values.Distinct();         }

        public IEnumerable<int> GetAllChildStatements()
        {
            return _db.Parent.Keys.Distinct(); 
        }


        // === Modifies ===
        public bool IsModifies(int stmt, string variable) =>
            _db.IsModifies(stmt, variable);

        public IEnumerable<string> GetModifiedVariables(int stmt) =>
            _db.Modifies.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();

        public IEnumerable<int> GetStatementsModifying(string variable) =>
            _db.Modifies.Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);

        public IEnumerable<int> GetAllStatementsModifyingAnything() =>
            _db.Modifies.Keys;
        public IEnumerable<string> GetAllModifiedVariables() =>
             _db.Modifies.Values.SelectMany(set => set).Distinct();

        // === Uses ===
        public bool IsUses(int stmt, string variable) =>
            _db.IsUses(stmt, variable);

        public IEnumerable<string> GetUsedVariables(int stmt) =>
            _db.Uses.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();

        public IEnumerable<int> GetStatementsUsing(string variable) =>
            _db.Uses.Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);

        public IEnumerable<int> GetAllStatementsUsingAnything()
        {
            return _db.Uses.Keys;
        }

        public IEnumerable<string> GetAllUsedVariables()
        {
            return _db.Uses.Values.SelectMany(vars => vars).Distinct();
        }

    }
}
