using Atsi.Structures.SIMPLE.Statements;

namespace Atsi.Structures.SIMPLE
{
    public class Procedure
    {
        public string Name { get; }
        public List<Statement> StatementsList { get; }
        public Procedure(string name, List<Statement> statementsList)
        {
            Name = name;
            StatementsList = statementsList;
        }
    }
}
