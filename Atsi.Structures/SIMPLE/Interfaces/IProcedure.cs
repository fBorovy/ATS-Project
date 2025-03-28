using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atsi.Structures.SIMPLE.Interfaces
{
    public interface IProcedure
    {
        HashSet<string> GetAllModifiedVariables(Procedure procedure); //Zwraca wszystkie zmienne modyfikowane w procedurze  
        HashSet<string> GetAllUsedVariables(Procedure procedure); // Zwraca wszystkie zmienne używane w procedurze 
        List<string> GetAllAssignmentStatements(Procedure procedure); //Zwraca wszystkie instrukcje przypisań w procedurze
    }
}
