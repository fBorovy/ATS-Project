namespace Atsi.Structures.Utils.Enums
{
    public enum DictAvailableArythmeticSymbols : byte
    {
        Plus = 1,
        Minus = 2,
        Times = 3,
        Divide = 4,
    }

    public static class OperatorExtensions
    {
        public static string GetSymbol(this DictAvailableArythmeticSymbols op)
        {
            return op switch
            {
                DictAvailableArythmeticSymbols.Plus => "+",
                DictAvailableArythmeticSymbols.Minus => "-",
                DictAvailableArythmeticSymbols.Times => "*",
                DictAvailableArythmeticSymbols.Divide => "/",
                _ => throw new ArgumentOutOfRangeException(nameof(op), op, null),
            };
        }
    }
}
