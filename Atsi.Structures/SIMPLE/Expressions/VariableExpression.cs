namespace Atsi.Structures.SIMPLE.Expressions
{
    public class VariableExpression(string variableName) : Expression
    {
        public string VariableName { get; } = variableName;

        public override HashSet<string> GetUsedVariables()
        {
            return [VariableName];
        }
    }
}
