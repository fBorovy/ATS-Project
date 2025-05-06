using Atsi.Structures.SIMPLE;
using System;
using System.Collections.Generic;

namespace Atsi.Structures.PKB
{
    public class PKBStorage
    {
        private static readonly Lazy<PKBStorage> _instance = new(() => new PKBStorage());
        public static PKBStorage Instance => _instance.Value;

        private readonly Dictionary<string, Procedure> Procedures = [];

        // Modifies
        private readonly Dictionary<string, HashSet<string>> ProcedureModifies = [];
        private readonly Dictionary<int, HashSet<string>> StatementModifies = [];

        // Uses
        private readonly Dictionary<string, HashSet<string>> ProcedureUses = [];
        private readonly Dictionary<int, HashSet<string>> StatementUses = [];

        // Calls
        private readonly Dictionary<string, HashSet<string>> Calls = [];
        private readonly Dictionary<string, HashSet<string>> CallsStar = [];

        // Parent
        private readonly Dictionary<int, int> Parent = [];
        private readonly Dictionary<int, HashSet<int>> ParentStar = [];

        // Follows
        private readonly Dictionary<int, int> Follows = [];
        private readonly Dictionary<int, HashSet<int>> FollowsStar = [];

        // Next
        private readonly Dictionary<int, HashSet<int>> Next = [];
        private readonly Dictionary<int, HashSet<int>> NextStar = [];

        // Affects
        private readonly Dictionary<int, HashSet<int>> Affects = [];
        private readonly Dictionary<int, HashSet<int>> AffectsStar = [];

        private static int _statementCounter = 1;
        public static int GetNextStatementNumber() => _statementCounter++;

        private PKBStorage() { }

        // --- Procedures ---
        public void AddProcedure(string name, Procedure procedure) => Procedures[name] = procedure;
        public Procedure? GetProcedure(string name) => Procedures.TryGetValue(name, out var proc) ? proc : null;

        // --- Follows ---
        public void AddFollows(int stmt1, int stmt2) => Follows[stmt1] = stmt2;
        public bool IsFollows(int stmt1, int stmt2) => Follows.TryGetValue(stmt1, out var next) && next == stmt2;
        public void AddFollowsStar(int stmt, int following)
        {
            if (!FollowsStar.ContainsKey(stmt)) FollowsStar[stmt] = new();
            FollowsStar[stmt].Add(following);
        }
        public bool IsFollowsStar(int stmt, int following) => FollowsStar.TryGetValue(stmt, out var set) && set.Contains(following);

        // --- Parent ---
        public void AddParent(int parentStmt, int childStmt) => Parent[childStmt] = parentStmt;
        public bool IsParent(int parentStmt, int childStmt) => Parent.TryGetValue(childStmt, out var parent) && parent == parentStmt;
        public void AddParentStar(int stmt, int descendant)
        {
            if (!ParentStar.ContainsKey(stmt)) ParentStar[stmt] = new();
            ParentStar[stmt].Add(descendant);
        }
        public bool IsParentStar(int stmt, int descendant) => ParentStar.TryGetValue(stmt, out var set) && set.Contains(descendant);

        // --- Modifies ---
        public void AddModifies(int stmt, string variable)
        {
            if (!StatementModifies.ContainsKey(stmt)) StatementModifies[stmt] = new();
            StatementModifies[stmt].Add(variable);
        }
        public void AddModifies(string proc, string variable)
        {
            if (!ProcedureModifies.ContainsKey(proc)) ProcedureModifies[proc] = new();
            ProcedureModifies[proc].Add(variable);
        }
        public bool IsModifies(int stmt, string variable) =>
            StatementModifies.TryGetValue(stmt, out var set) && set.Contains(variable);
        public bool IsModifies(string proc, string variable) =>
            ProcedureModifies.TryGetValue(proc, out var set) && set.Contains(variable);

        // --- Uses ---
        public void AddUses(int stmt, string variable)
        {
            if (!StatementUses.ContainsKey(stmt)) StatementUses[stmt] = new();
            StatementUses[stmt].Add(variable);
        }
        public void AddUses(string proc, string variable)
        {
            if (!ProcedureUses.ContainsKey(proc)) ProcedureUses[proc] = new();
            ProcedureUses[proc].Add(variable);
        }
        public bool IsUses(int stmt, string variable) =>
            StatementUses.TryGetValue(stmt, out var set) && set.Contains(variable);
        public bool IsUses(string proc, string variable) =>
            ProcedureUses.TryGetValue(proc, out var set) && set.Contains(variable);

        // --- Calls ---
        public void AddCalls(string caller, string callee)
        {
            if (!Calls.ContainsKey(caller)) Calls[caller] = new();
            Calls[caller].Add(callee);
        }
        public void AddCallsStar(string caller, string callee)
        {
            if (!CallsStar.ContainsKey(caller)) CallsStar[caller] = new();
            CallsStar[caller].Add(callee);
        }
        public bool IsCalls(string caller, string callee) =>
            Calls.TryGetValue(caller, out var set) && set.Contains(callee);
        public bool IsCallsStar(string caller, string callee) =>
            CallsStar.TryGetValue(caller, out var set) && set.Contains(callee);

        // --- Next ---
        public void AddNext(int stmt1, int stmt2)
        {
            if (!Next.ContainsKey(stmt1)) Next[stmt1] = new();
            Next[stmt1].Add(stmt2);
        }
        public bool IsNext(int stmt1, int stmt2) =>
            Next.TryGetValue(stmt1, out var set) && set.Contains(stmt2);
        public void AddNextStar(int stmt1, int stmt2)
        {
            if (!NextStar.ContainsKey(stmt1)) NextStar[stmt1] = new();
            NextStar[stmt1].Add(stmt2);
        }
        public bool IsNextStar(int stmt1, int stmt2) =>
            NextStar.TryGetValue(stmt1, out var set) && set.Contains(stmt2);

        // --- Affects ---
        public void AddAffects(int stmt1, int stmt2)
        {
            if (!Affects.ContainsKey(stmt1)) Affects[stmt1] = new();
            Affects[stmt1].Add(stmt2);
        }
        public bool IsAffects(int stmt1, int stmt2) =>
            Affects.TryGetValue(stmt1, out var set) && set.Contains(stmt2);
        public void AddAffectsStar(int stmt1, int stmt2)
        {
            if (!AffectsStar.ContainsKey(stmt1)) AffectsStar[stmt1] = new();
            AffectsStar[stmt1].Add(stmt2);
        }
        public bool IsAffectsStar(int stmt1, int stmt2) =>
            AffectsStar.TryGetValue(stmt1, out var set) && set.Contains(stmt2);
    }

}