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

        // === Parent ===
        public bool IsParent(int parentStmt, int childStmt) =>
            _db.IsParent(parentStmt, childStmt);

        public int? GetParent(int childStmt) =>
            _db.Parent.TryGetValue(childStmt, out var parent) ? parent : null;

        public IEnumerable<int> GetChildren(int parentStmt) =>
            _db.Parent.Where(kvp => kvp.Value == parentStmt).Select(kvp => kvp.Key);

        // === Modifies ===
        public bool IsModifies(int stmt, string variable) =>
            _db.IsModifies(stmt, variable);

        public IEnumerable<string> GetModifiedVariables(int stmt) =>
            _db.Modifies.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();

        public IEnumerable<int> GetStatementsModifying(string variable) =>
            _db.Modifies.Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);

        // === Uses ===
        public bool IsUses(int stmt, string variable) =>
            _db.IsUses(stmt, variable);

        public IEnumerable<string> GetUsedVariables(int stmt) =>
            _db.Uses.TryGetValue(stmt, out var vars) ? vars : Enumerable.Empty<string>();

        public IEnumerable<int> GetStatementsUsing(string variable) =>
            _db.Uses.Where(kvp => kvp.Value.Contains(variable)).Select(kvp => kvp.Key);
    }
}
