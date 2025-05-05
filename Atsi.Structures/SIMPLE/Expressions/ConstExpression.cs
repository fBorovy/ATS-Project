namespace Atsi.Structures.SIMPLE.Expressions
{
    public class ConstExpression(int value) : Expression
    {
        public int Value { get; } = value;

        public override HashSet<string> GetUsedVariables()
        {
            return [];
        }
    }
}
