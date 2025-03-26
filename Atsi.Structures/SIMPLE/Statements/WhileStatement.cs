namespace Atsi.Structures.SIMPLE.Statements
{
    public class WhileStatement : Statement
    {
        public string ConditionalVariableName { get; }
        public List<Statement> StatementsList { get; }

        public WhileStatement(string conditionalVariableName, List<Statement> statementsList)
        {
            ConditionalVariableName = conditionalVariableName;
            StatementsList = statementsList;
        }
    }
}
