using Atsi.Structures.Utils.Enums;

namespace Atsi.Structures.Simple
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Line { get; }
        
        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
        }

        public override string ToString()
        {
            return $"Token({Type}, \"{Value}\", Line {Line})";
        }
    }
}
