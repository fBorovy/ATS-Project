namespace Atsi.Structures.SIMPLE.Statements
{
    public class WhileStatement : Statement
    {
        public required string ConditionalVariableName { get; set; }
        public List<Statement> StatementsList { get; set; } = [];
    }
}
