using Atsi.Structures.SIMPLE.Expressions;

namespace Atsi.Structures.SIMPLE.Statements
{
    public class AssignStatement : Statement
    {
        public string VariableName { get; }
        public Expression Expression { get; }

        public AssignStatement(string variableName, Expression expression)
        {
            VariableName = variableName;
            Expression = expression;
        }
    }
}
