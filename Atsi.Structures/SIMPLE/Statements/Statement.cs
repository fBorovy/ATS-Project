namespace Atsi.Structures.SIMPLE.Statements
{
    public abstract class Statement
    {
        public int StatementNumber { get; set; }
        public required string ProcedureName { get; set; }
    }
}
