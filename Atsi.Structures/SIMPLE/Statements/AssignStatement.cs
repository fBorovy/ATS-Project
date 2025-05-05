using Atsi.Structures.SIMPLE.Expressions;

namespace Atsi.Structures.SIMPLE.Statements
{
    public class AssignStatement : Statement
    {
        public string VariableName { get; set; }
        public Expression Expression { get; set; }

        public AssignStatement(string VariableName, Expression expression) : base()
        {
            this.VariableName = VariableName;
            Expression = expression;
        }

        public AssignStatement(int statementNumber, string variableName, Expression expression) : base(statementNumber)
        {
            VariableName = variableName;
            Expression = expression;
        }
    }
}
