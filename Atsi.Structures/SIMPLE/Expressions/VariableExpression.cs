namespace Atsi.Structures.SIMPLE.Expressions
{
    public class VariableExpression : Expression
    {
        public string VariableName { get; }
        public VariableExpression(string variableName)
        {
            VariableName = variableName;
        }
        public override HashSet<string> GetUsedVariables()
        {
            return new HashSet<string> { VariableName };
        }
    }
}
