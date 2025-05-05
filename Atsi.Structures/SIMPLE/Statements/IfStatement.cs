namespace Atsi.Structures.SIMPLE.Statements
{
    public class IfStatement : Statement
    {
        public string VariableName { get; set; }
        public List<Statement> BodyStatements { get; set; }
        public List<Statement> ElseBodyStatements { get; set; }

        public IfStatement(string VariableName) : base()
        {
            this.VariableName = VariableName;
            BodyStatements = [];
            ElseBodyStatements = [];
        }

        public IfStatement(int StatementNumber, string VariableName, List<Statement> BodyStatements, List<Statement> ElseBodyStatements) : base(StatementNumber)
        {
            this.StatementNumber = StatementNumber;
            this.VariableName = VariableName;
            this.BodyStatements = BodyStatements;
            this.ElseBodyStatements = ElseBodyStatements;
        }
    }
}