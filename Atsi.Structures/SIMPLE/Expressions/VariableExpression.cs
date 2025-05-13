namespace Atsi.Structures.SIMPLE.Expressions
{
    public class VariableExpression(string variableName) : Expression
    {
        public string VariableName { get; set; } = variableName;

        public override HashSet<string> GetUsedVariables()
        {
            return [VariableName];
        }
    }
}
