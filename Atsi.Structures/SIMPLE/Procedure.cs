using Atsi.Structures.SIMPLE.Statements;

namespace Atsi.Structures.SIMPLE
{
    public class Procedure(string name)
    {
        public string Name { get; set; } = name;
        public List<Statement> StatementsList { get; set; } = [];
    }
}
