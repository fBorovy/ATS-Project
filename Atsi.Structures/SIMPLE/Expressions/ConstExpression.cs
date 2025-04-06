namespace Atsi.Structures.SIMPLE.Expressions
{
    public class ConstExpression : Expression
    {
        public int Value { get; }

        public ConstExpression(int value) 
        {
            Value = value;
        }
        public override HashSet<string> GetUsedVariables()
        {
            return [];
        }
    }
}
