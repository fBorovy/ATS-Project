namespace Atsi.Structures.SIMPLE.Statements
{
    public class WhileStatement : Statement
    {
        public string ConditionalVariableName { get; set; }
        public List<Statement> StatementsList { get; set; }

        public WhileStatement(string conditionalVariableName) : base()
        {
            ConditionalVariableName = conditionalVariableName;
            StatementsList = [];
        }

        public WhileStatement(int statementNumber, string conditionalVariableName, List<Statement> statementsList) : base(statementNumber)
        {
            ConditionalVariableName = conditionalVariableName;
            StatementsList = statementsList;
        }
    }
}
