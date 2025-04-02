using System.ComponentModel;
using System.Reflection;

namespace IDE.PQLParser;

public enum SynonymType
{
    Assign,
    Procedure,
    Statement,
    While,
    Pattern,
    Constant,
    Prog_line,
    Variable,
}