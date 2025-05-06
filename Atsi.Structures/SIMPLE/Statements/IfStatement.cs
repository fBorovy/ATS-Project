namespace Atsi.Structures.SIMPLE.Statements
{
    public class IfStatement : Statement
    {
        public string VariableName { get; set; }
        public List<Statement> ThenBodyStatements { get; set; }
        public List<Statement> ElseBodyStatements { get; set; }

        public IfStatement(string VariableName) : base()
        {
            this.VariableName = VariableName;
            ThenBodyStatements = [];
            ElseBodyStatements = [];
        }

        public IfStatement(int StatementNumber, string VariableName, List<Statement> ThenBodyStatements, List<Statement> ElseBodyStatements) : base(StatementNumber)
        {
            this.StatementNumber = StatementNumber;
            this.VariableName = VariableName;
            this.ThenBodyStatements = ThenBodyStatements;
            this.ElseBodyStatements = ElseBodyStatements;
        }
    }
}