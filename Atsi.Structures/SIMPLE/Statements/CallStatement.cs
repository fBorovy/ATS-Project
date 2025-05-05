namespace Atsi.Structures.SIMPLE.Statements
{
    public class CallStatement : Statement
    {
        public string ProcedureName { get; set; }

        public CallStatement(string ProcedureName) : base()
        {
            this.ProcedureName = ProcedureName;
        }

        public CallStatement(string ProcedureName, int StatementNumber) : base(StatementNumber)
        {
            this.ProcedureName = ProcedureName;
        }
    }
}