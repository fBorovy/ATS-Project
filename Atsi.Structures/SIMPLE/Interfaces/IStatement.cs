using Atsi.Structures.SIMPLE.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atsi.Structures.SIMPLE.Interfaces
{
    public interface IStatement
    {
        HashSet<string> GetModifiedVariables(Statement statement); // Zwraca zmienne modyfikowane przez instrukcję 
        HashSet<string> GetUsedVariables(Statement statement); // Zwraca zmienne używane przez instrukcję 
    }
}
