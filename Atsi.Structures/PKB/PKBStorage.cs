using Atsi.Structures.SIMPLE;
using System;
using System.Collections.Generic;

namespace Atsi.Structures.PKB
{
    public class PKBStorage
    {
        private static readonly Lazy<PKBStorage> _instance = new(() => new PKBStorage());
        public static PKBStorage Instance => _instance.Value;

        internal Dictionary<string, Procedure> Procedures;
        internal Dictionary<int, int> Follows;
        internal Dictionary<int, int> Parent;
        internal Dictionary<int, HashSet<string>> Modifies;
        internal Dictionary<int, HashSet<string>> Uses;

        private PKBStorage()
        {
            Procedures = new Dictionary<string, Procedure>();
            Follows = new Dictionary<int, int>();
            Parent = new Dictionary<int, int>();
            Modifies = new Dictionary<int, HashSet<string>>();
            Uses = new Dictionary<int, HashSet<string>>();
        }

        // Procedure Management
        public void AddProcedure(string name, Procedure procedure)
        {
            Procedures[name] = procedure;
        }

        public Procedure? GetProcedure(string name)
        {
            return Procedures.TryGetValue(name, out Procedure? proc) ? proc : null;
        }

        // Follows Relationship
        public void AddFollows(int stmt1, int stmt2)
        {
            Follows[stmt1] = stmt2;
        }

        public bool IsFollows(int stmt1, int stmt2)
        {
            return Follows.TryGetValue(stmt1, out int next) && next == stmt2;
        }

        // Parent Relationship
        public void AddParent(int parentStmt, int childStmt)
        {
            Parent[childStmt] = parentStmt;
        }

        public bool IsParent(int parentStmt, int childStmt)
        {
            return Parent.TryGetValue(childStmt, out int parent) && parent == parentStmt;
        }

        // Modifies Relationship
        public void AddModifies(int stmt, string variable)
        {
            if (!Modifies.ContainsKey(stmt))
                Modifies[stmt] = new HashSet<string>();
            Modifies[stmt].Add(variable);
        }

        public bool IsModifies(int stmt, string variable)
        {
            return Modifies.ContainsKey(stmt) && Modifies[stmt].Contains(variable);
        }

        // Uses Relationship
        public void AddUses(int stmt, string variable)
        {
            if (!Uses.ContainsKey(stmt))
                Uses[stmt] = new HashSet<string>();
            Uses[stmt].Add(variable);
        }

        public bool IsUses(int stmt, string variable)
        {
            return Uses.ContainsKey(stmt) && Uses[stmt].Contains(variable);
        }
    }
}