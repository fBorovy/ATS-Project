namespace Atsi.Structures.SIMPLE.Statements
{
    public abstract class Statement
    {
        public int StatementNumber { get; }
        protected Statement(int statementNumber)
        {
            StatementNumber = statementNumber;
        }
    }
}
