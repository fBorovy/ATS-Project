using System;
using System.ComponentModel;
using System.Reflection;

namespace IDE.PQLParser;

// (do zbudowania drzewa zapytania) - ta klasa przechowuje to, czym zapytanie się zaczyna, np: procedure q, assign a
public struct Synonym
{
    public SynonymType Type { get; } 
    public string Name { get; }

    public Synonym(SynonymType type, string name)
    {
        Type = type;
        Name = name;
    }

    public bool IsProcedure() => Type == SynonymType.Procedure;
    public bool IsAssignment() => Type == SynonymType.Assign;
    public bool IsWhile() => Type == SynonymType.While;
    public bool IsStatement() => Type == SynonymType.Statement;
}