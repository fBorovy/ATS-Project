namespace Atsi.Structures.SIMPLE.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public string Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, string _operator, Expression right)
        {
            Left = left;
            Operator = _operator;
            Right = right;
        }
    }
}
