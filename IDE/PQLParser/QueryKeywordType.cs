namespace IDE.PQLParser;

public enum QueryKeywordType
{
    Procedure, Call, Statement, Assign, While, If, Variable, Constant, Prog_line,
    Select, SuchThat, Follows, FollowsT, Parent, ParentT, Uses, UsesT, Modifies, ModifiesT, Calls, CallsT, With, Attribute, And,
    Identifier, String ,Number, Comma, Equals, Joker, OpenParen, CloseParen, End
}
