using Atsi.Structures.SIMPLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atsi.Structures.PKB.Explorer
{
    public interface IPKBQuery
    {
        // === Procedure ===
        Procedure? GetProcedure(string name);  // Zwraca procedurę o podanej nazwie
        IEnumerable<string> GetAllProcedureNames();  // Zwraca wszystkie nazwy procedur zapisane w PKB

        // === Follows ===
        bool IsFollows(int stmt1, int stmt2);   // Sprawdza, czy instrukcja stmt2 bezpośrednio następuje po stmt1
        int? GetFollows(int stmt1);    // Zwraca instrukcję, która następuje bezpośrednio po stmt1
        int? GetFollowedBy(int stmt2);   // Zwraca instrukcję, po której następuje stmt2 (czyli odwrotność GetFollows)

        // === Parent ===
        bool IsParent(int parentStmt, int childStmt);  // Sprawdza, czy parentStmt jest rodzicem instrukcji childStmt
        int? GetParent(int childStmt);   // Zwraca numer instrukcji, która jest rodzicem danej
        IEnumerable<int> GetChildren(int parentStmt);   // Zwraca wszystkie instrukcje będące dziećmi parentStmt

        // === Modifies ===
        bool IsModifies(int stmt, string variable);   // Sprawdza, czy instrukcja stmt modyfikuje zmienną variable
        IEnumerable<string> GetModifiedVariables(int stmt);   // Zwraca wszystkie zmienne modyfikowane przez daną instrukcję
        IEnumerable<int> GetStatementsModifying(string variable);   // Zwraca wszystkie instrukcje, które modyfikują daną zmienną

        // === Uses ===
        bool IsUses(int stmt, string variable);  // Sprawdza, czy instrukcja stmt używa zmiennej variable
        IEnumerable<string> GetUsedVariables(int stmt);  // Zwraca wszystkie zmienne używane w danej instrukcji
        IEnumerable<int> GetStatementsUsing(string variable);   // Zwraca wszystkie instrukcje, które używają danej zmiennej
    }
}
