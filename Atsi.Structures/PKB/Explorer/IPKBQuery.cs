using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Statements;
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

        /// <summary>Zwraca nazwy wszystkich procedur, które modyfikują przynajmniej jedną zmienną</summary>
        IEnumerable<string> GetAllProceduresModifyingAnything();

        /// <summary>Zwraca wszystkie procedury, które używają przynajmniej jednej zmiennej</summary>
        IEnumerable<string> GetAllProceduresUsingAnything();

        /// <summary>Zwraca wszystkie procedury, które wywołują inne procedury (czyli występują jako klucze w relacji Calls)</summary>

        IEnumerable<string> GetAllCallingProcedures();

        /// <summary>Zwraca unikalne procedury, które są wywoływane w relacji Calls</summary>
        IEnumerable<string> GetAllCalledProcedures();

        /// <summary>Zwraca wszystkie procedury, które pośrednio lub bezpośrednio wywołują daną procedurę (Calls*)</summary>
        IEnumerable<string> GetCallingProceduresT(string callee);

        /// <summary>// Zwraca wszystkie procedury, które są pośrednio lub bezpośrednio wywoływane przez daną procedurę (Calls*)</summary>
        IEnumerable<string> GetCalledProceduresT(string caller);

        /// <summary>dla danej procedury (procName) zwraca zbiór wszystkich używanych zmiennych</summary>
        IEnumerable<string> GetUsedVariablesByProcedure(string procedureName);


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

        /// <summary>//Zwraca instrukcje danego typu, które następują po podanej instrukcji w relacji Follows*</summary>
        IEnumerable<int> GetAllFollowingStatements(int stmt1, string statementType);

        /// <summary>Zwraca wszystkie instrukcje, które prowadzą do stmt2 (Follows*).</summary>
        IEnumerable<int> GetAllStatementsLeadingTo(int s2);

        /// <summary>Zwraca instrukcje danego typu, które poprzedzają podaną instrukcję w relacji Follows*</summary>
        IEnumerable<int> GetAllStatementsLeadingTo(int stmt2, string statementType);


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

        /// <summary>Zwraca wszystkie zmienne używane przez instrukcje danego typu</summary>
        IEnumerable<string> GetAllUsedVariables(string statementType);

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

        // === Metody zwracające numery wszystkich typów statementów w programie ===

        /// <summary>Zwraca numery wszystkich instrukcji typu while w programie</summary>
        IEnumerable<int> GetAllWhiles();

        /// <summary>Zwraca numery wszystkich instrukcji typu if w programie</summary>
        IEnumerable<int> GetAllIfs();

        /// <summary>Zwraca numery wszystkich instrukcji przypisania (assign) w programie</summary>
        IEnumerable<int> GetAllAssigns();

        /// <summary>Zwraca numery wszystkich calls w programie</summary>
        IEnumerable<int> GetAllCallsNumbers();

        /// <summary>Zwraca numery wszystkich instrukcji w programie, niezależnie od typu</summary>
        IEnumerable<int> GetAllStatements();

        /// <summary>Zwraca numery instrukcji przypisania, które używają podanej zmiennej w wyrażeniu po prawej stronie</summary>
        IEnumerable<int> GetAssignmentsUsing(string variable);

        /// <summary>Zwraca numery instrukcji if, które używają podanej zmiennej jako warunku</summary>

        IEnumerable<int> GetIfsUsing(string variable);

        /// <summary>Zwraca numery instrukcji while, które używają podanej zmiennej jako warunku</summary>

        IEnumerable<int> GetWhilesUsing(string variable);

        /// <summary>Zwraca wszystkie instrukcje while, które występują jako pierwsze w relacji Follows (czyli mają następnika)</summary>
        IEnumerable<int> GetAllFollowedWhiles();

        /// <summary>Zwraca wszystkie instrukcje przypisania, które używają przynajmniej jednej zmiennej</summary>
        IEnumerable<int> GetAllAssignmentsUsingAnything();

        /// <summary>Zwraca wszystkie instrukcje while, które używają jakiejkolwiek zmiennej w warunku</summary>

        IEnumerable<int> GetAllWhilesUsingAnything();

        /// <summary>Zwraca wszystkie instrukcje if, które używają jakiejkolwiek zmiennej w warunku</summary>
        IEnumerable<int> GetAllIfsUsingAnything();

        /// <summary>Zwraca wszystkie instrukcje typu if, które są rodzicami innych instrukcji</summary>

        IEnumerable<int> GetAllParentIfs();

        /// <summary> Zwraca wszystkie instrukcje typu while, które są rodzicami innych instrukcji</summary>
        IEnumerable<int> GetAllParentWhiles();

        /// <summary> Zwraca zbiór zmiennych używanych w instrukcji przypisania o podanym numerze</summary>
        IEnumerable<string> GetUsedVariablesByAssign(int stmtNumber);

        /// <summary> Zwraca zmienną warunkową używaną w instrukcji while o podanym numerze</summary>
        IEnumerable<string> GetUsedVariablesByWhile(int stmtNumber);

        /// <summary> Zwraca zmienną warunkową używaną w instrukcji if o podanym numerze</summary>
        IEnumerable<string> GetUsedVariablesByIf(int stmtNumber);

        /// <summary>Zwraca bezpośrednie dzieci instrukcji if – z gałęzi then i else</summary>
        IEnumerable<int> GetChildrenOfIf(int ifStmt);

        /// <summary> Zwraca bezpośrednie dzieci instrukcji while – ciało pętli</summary>
        IEnumerable<int> GetChildrenOfWhile(int whileStmt);

        /// <summary> Zwraca wszystkie zagnieżdżone (rekurencyjnie) instrukcje wewnątrz if'a</summary>
        IEnumerable<int> GetAllNestedStatementsInIfT(int ifStmt);

        /// <summary> Zwraca wszystkie zagnieżdżone (rekurencyjnie) instrukcje wewnątrz if'a</summary>
        IEnumerable<int> GetAllNestedStatementsInWhileT(int whileStmt);

        /// <summary> Zwraca bezpośrednich potomków wszystkich instrukcji typu if (z Then i Else)</summary>
        IEnumerable<int> GetAllChildStatementsOfIfs();

        /// <summary>
        /// Zwraca bezpośrednich potomków wszystkich instrukcji typu while (ciała pętli)
        /// </summary>
        IEnumerable<int> GetAllChildStatementsOfWhiles();

        /// <summary> Zwraca wszystkie zagnieżdżone (rekurencyjnie) potomki wszystkich ifów</summary>
        IEnumerable<int> GetAllChildStatementsOfIfsT();

        /// <summary> Zwraca wszystkie zagnieżdżone (rekurencyjnie) potomki wszystkich while'ów</summary>
        IEnumerable<int> GetAllChildStatementsOfWhilesT();

        /// <summary>Zwraca numer instrukcji if, która bezpośrednio poprzedza podaną instrukcję</summary>
        int? GetFollowedByIf(int followingStmtNumber);

        /// <summary> Zwraca numer instrukcji while, która bezpośrednio poprzedza podaną instrukcję</summary>
        int? GetFollowedByWhile(int followingStmtNumber);

        /// <summary>Zwraca numer instrukcji call, która bezpośrednio poprzedza podaną instrukcję</summary>
        int? GetFollowedByCall(int followingStmtNumber);

        /// <summary>Zwraca numer instrukcji assign, która bezpośrednio poprzedza podaną instrukcję</summary>
        int? GetFollowedByAssign(int followingStmtNumber);

        /// <summary>
        /// Zwraca wszystkie instrukcje danego typu, które występują jako źródła relacji Follows (czyli mają następnika).
        /// </summary>
        /// <param name="nodeType">Typ instrukcji: "While", "If", "Assign", "Call".</param>
        /// <returns>Lista numerów instrukcji będących źródłami Follows dla podanego typu lub wszystkich, jeśli typ nieznany.</returns>
        IEnumerable<int> GetFollowedStatementsByType(string nodeType);

        /// <summary>
        /// Zwraca numer instrukcji typu If, która bezpośrednio następuje po podanej instrukcji w relacji Follows.
        /// </summary>
        int? GetIfFollowing(int stmt);

        /// <summary>
        /// Zwraca numer instrukcji typu While, która bezpośrednio następuje po podanej instrukcji w relacji Follows.
        /// </summary>
        int? GetWhileFollowing(int stmt);

        /// <summary>
        /// Zwraca numer instrukcji typu Call, która bezpośrednio następuje po podanej instrukcji w relacji Follows.
        /// </summary>
        int? GetCallFollowing(int stmt);

        /// <summary>
        /// Zwraca numer instrukcji typu Assign, która bezpośrednio następuje po podanej instrukcji w relacji Follows.
        /// </summary>
        int? GetAssignFollowing(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu If, które występują po podanej instrukcji w relacji Follows*.
        /// </summary>

        IEnumerable<int> GetIfsFollowingT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu While, które występują po podanej instrukcji w relacji Follows*.
        /// </summary>

        IEnumerable<int> GetWhilesFollowingT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu Calls, które występują po podanej instrukcji w relacji Follows*.
        /// </summary>

        IEnumerable<int> GetCallsFollowingT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu Assign, które występują po podanej instrukcji w relacji Follows*.
        /// </summary>

        IEnumerable<int> GetAssignsFollowingT(int stmt);

        /// <summary>
        /// Zwraca wszystkie Stmt, które występują po podanej instrukcji w relacji Follows*.
        /// </summary>

        IEnumerable<int> GetStmtsFollowingT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu If, które poprzedzają daną instrukcję w relacji Follows*.
        /// </summary>
        /// <param name="stmt">Numer instrukcji docelowej.</param>
        /// <returns>Zbiór instrukcji typu If będących poprzednikami w Follows*.</returns>
        IEnumerable<int> GetFollowedByIfsT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu While, które poprzedzają daną instrukcję w relacji Follows*.
        /// </summary>
        /// <param name="stmt">Numer instrukcji docelowej.</param>
        /// <returns>Zbiór instrukcji typu While będących poprzednikami w Follows*.</returns>
        IEnumerable<int> GetFollowedByWhilesT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu Calls, które poprzedzają daną instrukcję w relacji Follows*.
        /// </summary>
        /// <param name="stmt">Numer instrukcji docelowej.</param>
        /// <returns>Zbiór instrukcji typu Call będących poprzednikami w Follows*.</returns>
        IEnumerable<int> GetFollowedByCallsT(int stmt);

        /// <summary>
        /// Zwraca wszystkie instrukcje typu Assign, które poprzedzają daną instrukcję w relacji Follows*.
        /// </summary>
        /// <param name="stmt">Numer instrukcji docelowej.</param>
        /// <returns>Zbiór instrukcji typu Assign będących poprzednikami w Follows*.</returns>
        IEnumerable<int> GetFollowedByAssignsT(int stmt);

        /// <summary>
        /// Zwraca wszystkie Stmts, które poprzedzają daną instrukcję w relacji Follows*.
        /// </summary>
        /// <param name="stmt">Numer instrukcji docelowej.</param>
        /// <returns>Zbiór Stmts będących poprzednikami w Follows*.</returns>
        IEnumerable<int> GetFollowedByStmtsT(int stmt);

        /// <summary>
        /// Zwraca instrukcje danego typu, które są śledzone przez instrukcje innego typu w relacji Follows
        /// </summary>
        IEnumerable<int> GetFollowedStatementsByType(string nodeType, string followedType);
    }
}
