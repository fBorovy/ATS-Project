using Atsi.Structures.SIMPLE.Expressions;

namespace Atsi.Structures.SIMPLE.Statements
{
    public class AssignStatement : Statement
    {
        public required string VariableName { get; set; }
        public required Expression Expression { get; set; }

    }
}
