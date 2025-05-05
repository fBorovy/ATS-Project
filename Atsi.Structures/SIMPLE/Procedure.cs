using Atsi.Structures.SIMPLE.Statements;

namespace Atsi.Structures.SIMPLE
{
    public class Procedure(string name)
    {
        public string Name { get; } = name;
        public List<Statement> StatementsList { get; } = [];
    }
}
