using Atsi.Structures.SIMPLE;
using System.Collections.Generic;

namespace Atsi.Structures.PKB.Explorer
{
    public interface IPKBQuery
    {
        // === Procedure ===

        /// <summary>Zwraca procedurę o podanej nazwie.</summary>
        Procedure? GetProcedure(string name);

        /// <summary>Zwraca wszystkie nazwy procedur zapisanych w PKB.</summary>
        IEnumerable<string> GetAllProcedureNames();


        // === Follows ===

        /// <summary>Sprawdza, czy stmt2 bezpośrednio następuje po stmt1 (Follows).</summary>
        bool IsFollows(int stmt1, int stmt2);

        /// <summary>Zwraca instrukcję, która następuje bezpośrednio po stmt1.</summary>
        int? GetFollows(int stmt1);

        /// <summary>Zwraca instrukcję, po której następuje stmt2.</summary>
        int? GetFollowedBy(int stmt2);

        /// <summary>Zwraca wszystkie instrukcje, które mają następnik (lewa strona relacji Follows).</summary>
        IEnumerable<int> GetAllFollowsSources();

        /// <summary>Zwraca wszystkie instrukcje, które są następnikiem innej (prawa strona relacji Follows).</summary>
        IEnumerable<int> GetAllFollowsTargets();

        /// <summary>Sprawdza, czy istnieje ścieżka Follows* między stmt1 a stmt2 (przechodnio).</summary>
        bool IsFollowsTransitive(int s1, int s2);

        /// <summary>Zwraca wszystkie instrukcje, które występują po stmt1 w kolejności wykonywania programu (Follows*).</summary>
        IEnumerable<int> GetAllFollowingStatements(int s1);

        /// <summary>Zwraca wszystkie instrukcje, które prowadzą do stmt2 (Follows*).</summary>
        IEnumerable<int> GetAllStatementsLeadingTo(int s2);


        // === Parent ===

        /// <summary>Sprawdza, czy parentStmt jest bezpośrednim rodzicem childStmt.</summary>
        bool IsParent(int parentStmt, int childStmt);

        /// <summary>Zwraca bezpośredniego rodzica instrukcji.</summary>
        int? GetParent(int childStmt);

        /// <summary>Zwraca bezpośrednie dzieci instrukcji (Parent).</summary>
        IEnumerable<int> GetChildren(int parentStmt);

        /// <summary>Zwraca wszystkie pary (rodzic, dziecko) w relacji Parent.</summary>
        IEnumerable<(int parent, int child)> GetAllParentPairs();

        /// <summary>Sprawdza, czy child znajduje się wewnątrz parenta bezpośrednio lub zagnieżdżony głębiej (Parent*).</summary>
        bool IsNestedIn(int parent, int child);

        /// <summary>Zwraca wszystkie instrukcje zagnieżdżone wewnątrz parenta (Parent*).</summary>
        IEnumerable<int> GetAllNestedStatements(int parent);

        /// <summary>Zwraca wszystkie przodki dla danej instrukcji (ścieżka Parent* w górę).</summary>
        IEnumerable<int> GetAllParentStatements(int stmt);

        /// <summary>Zwraca wszystkie pary (parent, descendant) w relacji Parent*.</summary>
        IEnumerable<(int parent, int descendant)> GetAllNestedPairs();

        /// <summary>Zwraca wszystkie instrukcje, które są rodzicem czegokolwiek.</summary>
        IEnumerable<int> GetAllParentStatements();

        /// <summary>Zwraca wszystkie instrukcje, które mają jakiegoś rodzica.</summary>
        IEnumerable<int> GetAllChildStatements();


        // === Modifies (Statement) ===

        /// <summary>Sprawdza, czy dana instrukcja modyfikuje zmienną.</summary>
        bool IsModifies(int stmt, string variable);

        /// <summary>Zwraca zbiór zmiennych modyfikowanych przez daną instrukcję.</summary>
        IEnumerable<string> GetModifiedVariables(int stmt);

        /// <summary>Zwraca wszystkie instrukcje, które modyfikują wskazaną zmienną.</summary>
        IEnumerable<int> GetStatementsModifying(string variable);

        /// <summary>Zwraca wszystkie instrukcje, które modyfikują jakiekolwiek zmienne.</summary>
        IEnumerable<int> GetAllStatementsModifyingAnything();

        /// <summary>Zwraca wszystkie zmienne modyfikowane w programie.</summary>
        IEnumerable<string> GetAllModifiedVariables();


        // === Uses (Statement) ===

        /// <summary>Sprawdza, czy dana instrukcja używa zmiennej.</summary>
        bool IsUses(int stmt, string variable);

        /// <summary>Zwraca zmienne używane w danej instrukcji.</summary>
        IEnumerable<string> GetUsedVariables(int stmt);

        /// <summary>Zwraca wszystkie instrukcje, które używają wskazanej zmiennej.</summary>
        IEnumerable<int> GetStatementsUsing(string variable);

        /// <summary>Zwraca wszystkie instrukcje, które używają jakichkolwiek zmiennych.</summary>
        IEnumerable<int> GetAllStatementsUsingAnything();

        /// <summary>Zwraca wszystkie zmienne używane w programie.</summary>
        IEnumerable<string> GetAllUsedVariables();


        // === Modifies (Procedure) ===

        /// <summary>Sprawdza, czy procedura modyfikuje zmienną.</summary>
        bool IsModifies(string procedure, string variable);

        /// <summary>Zwraca zbiór zmiennych modyfikowanych przez procedurę.</summary>
        IEnumerable<string> GetModifiedVariables(string procedure);

        /// <summary>Zwraca procedury, które modyfikują wskazaną zmienną.</summary>
        IEnumerable<string> GetProceduresModifying(string variable);


        // === Uses (Procedure) ===

        /// <summary>Sprawdza, czy procedura używa zmiennej.</summary>
        bool IsUses(string procedure, string variable);

        /// <summary>Zwraca zbiór zmiennych używanych przez procedurę.</summary>
        IEnumerable<string> GetUsedVariables(string procedure);

        /// <summary>Zwraca procedury, które używają wskazanej zmiennej.</summary>
        IEnumerable<string> GetProceduresUsing(string variable);


        // === Calls ===

        /// <summary>Sprawdza, czy procedura caller wywołuje procedurę callee (bezpośrednio).</summary>
        bool IsCalls(string caller, string callee);

        /// <summary>Sprawdza, czy caller wywołuje callee bezpośrednio lub pośrednio (Calls*).</summary>
        bool IsCallsStar(string caller, string callee);

        /// <summary>Zwraca procedury wywoływane przez daną procedurę (Calls).</summary>
        IEnumerable<string> GetCalledProcedures(string caller);

        /// <summary>Zwraca procedury, które wywołują wskazaną procedurę.</summary>
        IEnumerable<string> GetCallingProcedures(string callee);

        /// <summary>Zwraca wszystkie pary relacji Calls (bezpośrednio)</summary>
        IEnumerable<(string caller, string callee)> GetAllCalls();

        /// <summary>Zwraca wszystkie pary relacji Calls* (przechodnio)</summary>
        public IEnumerable<(string caller, string callee)> GetAllCallsStar();


        // === Next ===

        /// <summary>Sprawdza, czy stmt2 może być wykonana bezpośrednio po stmt1.</summary>
        bool IsNext(int stmt1, int stmt2);

        /// <summary>Sprawdza relację Next* – czy istnieje ścieżka wykonania z stmt1 do stmt2.</summary>
        bool IsNextStar(int stmt1, int stmt2);

        /// <summary>Zwraca wszystkie instrukcje następujące po stmt1.</summary>
        IEnumerable<int> GetNextStatements(int stmt1);

        /// <summary>Zwraca wszystkie instrukcje, które mogą prowadzić do stmt2.</summary>
        IEnumerable<int> GetPreviousStatements(int stmt2);


        // === Affects ===

        /// <summary>Sprawdza, czy stmt1 wpływa na stmt2 przez modyfikację tej samej zmiennej.</summary>
        bool IsAffects(int stmt1, int stmt2);

        /// <summary>Sprawdza relację Affects* – pośrednie i bezpośrednie wpływy.</summary>
        bool IsAffectsStar(int stmt1, int stmt2);

        /// <summary>Zwraca instrukcje, na które wpływa stmt1.</summary>
        IEnumerable<int> GetAffectedStatements(int stmt1);

        /// <summary>Zwraca instrukcje, które wpływają na stmt2.</summary>
        IEnumerable<int> GetAffectingStatements(int stmt2);
    }
}
