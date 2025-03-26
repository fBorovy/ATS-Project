namespace Atsi.Structures.SIMPLE.Expressions
{
    public class VariableExpression : Expression
    {
        public string VariableName { get; }
        public VariableExpression(string variableName)
        {
            VariableName = variableName;
        }
    }
}
