using System.ComponentModel.DataAnnotations;

namespace Atsi.Structures.SIMPLE.Statements
{
    public class IfStatement : Statement
    {
        public required string ConditionalVariableName { get; set; }
        public List<Statement> ThenBodyStatements { get; set; } = [];
        public List<Statement> ElseBodyStatements { get; set; } = [];
    }
}