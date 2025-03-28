using Atsi.Structures.SIMPLE.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atsi.Structures.SIMPLE.Interfaces
{
    public interface IExpression
    {
        HashSet<string> GetUsedVariables(Expression expression); // Zwraca zbiór zmiennych używanych w wyrażeniu
        List<int> GetConstants(Expression expression); // Zwraca listę stałych użytych w wyrażeniu
    }
}
