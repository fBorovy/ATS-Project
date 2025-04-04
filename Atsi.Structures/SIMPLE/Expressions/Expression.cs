namespace Atsi.Structures.SIMPLE.Expressions
{
    public abstract class Expression
    {
        public abstract HashSet<string> GetUsedVariables();
    }
}
