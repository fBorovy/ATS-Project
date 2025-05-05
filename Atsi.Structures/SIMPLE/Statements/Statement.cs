namespace Atsi.Structures.SIMPLE.Statements
{
    public abstract class Statement
    {
        public int StatementNumber { get; set; }
        protected Statement() { }
        protected Statement(int statementNumber)
        {
            StatementNumber = statementNumber;
        }
    }
}
