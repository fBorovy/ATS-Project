using Atsi.Structures.SIMPLE.Expressions;

namespace Atsi.Structures.SIMPLE.Statements
{
    public class AssignStatement : Statement
    {
        public string VariableName { get; }
        public Expression Expression { get; }

        public AssignStatement(int statementNumber, string variableName, Expression expression) : base(statementNumber)
        {
            VariableName = variableName;
            Expression = expression;
        }
    }
}
