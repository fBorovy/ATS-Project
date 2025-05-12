using Atsi.Structures.Utils.Enums;

namespace Atsi.Structures.SIMPLE.Expressions
{
    public class BinaryExpression(Expression left, DictAvailableArythmeticSymbols _operator, Expression right) : Expression
    {
        public Expression Left { get; set; } = left;
        public DictAvailableArythmeticSymbols Operator { get; set; } = _operator;
        public Expression Right { get; set; } = right;

        public override HashSet<string> GetUsedVariables()
        {
            HashSet<string> vars = [];
            vars.UnionWith(Left.GetUsedVariables());
            vars.UnionWith(Right.GetUsedVariables());
            return vars;
        }
    }
}