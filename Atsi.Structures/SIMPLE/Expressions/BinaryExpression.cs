using Atsi.Structures.Utils.Enums;

namespace Atsi.Structures.SIMPLE.Expressions
{
    public class BinaryExpression(Expression left, DictAvailableArythmeticSymbols _operator, Expression right) : Expression
    {
        public Expression Left { get; } = left;
        public DictAvailableArythmeticSymbols Operator { get; } = _operator;
        public Expression Right { get; } = right;

        public override HashSet<string> GetUsedVariables()
        {
            HashSet<string> vars = [];
            vars.UnionWith(Left.GetUsedVariables());
            vars.UnionWith(Right.GetUsedVariables());
            return vars;
        }
    }
}