namespace Atsi.Structures.Utils.Enums
{
    public enum DictAvailableArythmeticSymbols : byte
    {
        Plus = 1,
        Minus = 2,
        Times = 3
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
                _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
            };
        }
    }
}
