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
        public IEnumerable<int> GetAllFollowsSources(); // Zwraca wszystkie instrukcje, które mają następniki (s1 w Follows(s1, s2))

        public IEnumerable<int> GetAllFollowsTargets();   // Zwraca wszystkie instrukcje, które są następnikiem innej (s2 w Follows(s1, s2))

        // metody przechodnie follows
        bool IsFollowsTransitive(int s1, int s2); // Sprawdza, czy jedna instrukcja może prowadzić do drugiej w kolejności wykonywania programu (bezpośrednio lub przez inne instrukcje)

        IEnumerable<int> GetAllFollowingStatements(int s1); // Zwraca wszystkie instrukcje, które występują po wskazanej instrukcji w programie (bezpośrednio lub dalej)
        IEnumerable<int> GetAllStatementsLeadingTo(int s2); // Zwraca wszystkie instrukcje, które występują przed wskazaną instrukcją w programie (bezpośrednio lub wcześniej)

        // === Parent ===
        bool IsParent(int parentStmt, int childStmt);  // Sprawdza, czy parentStmt jest rodzicem instrukcji childStmt
        int? GetParent(int childStmt);   // Zwraca numer instrukcji, która jest rodzicem danej
        IEnumerable<int> GetChildren(int parentStmt);   // Zwraca wszystkie instrukcje będące dziećmi parentStmt
        IEnumerable<(int parent, int child)> GetAllParentPairs(); // Zwraca wszystkie pary instrukcji w relacji Parent(p, c)

        // metody przechodnie parent
        bool IsNestedIn(int parent, int child);  // Sprawdza, czy jedna instrukcja (child) znajduje się wewnątrz innej (parent), bezpośrednio lub zagnieżdżona głębiej
        IEnumerable<int> GetAllNestedStatements(int parent); // Zwraca wszystkie instrukcje, które są zagnieżdżone wewnątrz danej na dowolnym poziomie
        IEnumerable<int> GetAllParentStatements(int stmt);   // Zwraca wszystkie nadrzędne instrukcje dla podanej w których się znajduje
        IEnumerable<(int parent, int descendant)> GetAllNestedPairs(); // Zwraca wszystkie pary instrukcji w relacji Parent*(p, c) (przechodnio)


        // === Modifies ===
        bool IsModifies(int stmt, string variable);   // Sprawdza, czy instrukcja stmt modyfikuje zmienną variable
        IEnumerable<string> GetModifiedVariables(int stmt);   // Zwraca wszystkie zmienne modyfikowane przez daną instrukcję
        IEnumerable<int> GetStatementsModifying(string variable);   // Zwraca wszystkie instrukcje, które modyfikują daną zmienną
        IEnumerable<int> GetAllStatementsModifyingAnything(); // Zwraca wszystkie instrukcje, które modyfikują jakąkolwiek zmienną
                                                             
        IEnumerable<string> GetAllModifiedVariables(); // Zwraca wszystkie zmienne, które są modyfikowane w całym programie


        // === Uses ===
        bool IsUses(int stmt, string variable);  // Sprawdza, czy instrukcja stmt używa zmiennej variable
        IEnumerable<string> GetUsedVariables(int stmt);  // Zwraca wszystkie zmienne używane w danej instrukcji
        IEnumerable<int> GetStatementsUsing(string variable);   // Zwraca wszystkie instrukcje, które używają danej zmiennej
        IEnumerable<int> GetAllStatementsUsingAnything(); // Zwraca wszystkie instrukcje, które używają jakiejkolwiek zmiennej
        IEnumerable<string> GetAllUsedVariables(); // Zwraca wszystkie zmienne używane w programie

    }
}
