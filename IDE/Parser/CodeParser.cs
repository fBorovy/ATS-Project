using System.Text.RegularExpressions;

using Atsi.Domain.Extensions;
using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Statements;

namespace IDE.Parser;

public class CodeParser
{
    private string path;
    private List<string> words;
    private IEnumerator<string> iterator;

    public CodeParser(string path)
    {
        this.path = path;
    }

    public bool ReadFile()
    {
        string? content =  File.ReadAllText(this.path);
        if (content == null) return false;
        this.words = Regex
            .Split(content, @"(?<=\S)(?=\s)|(?<=\s)(?=\S)|(?<=;)|(?=;)")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToList();
        this.iterator = words.GetEnumerator();
        return this.words.Any();
    }

    public bool Parse()
    {
        try
        {
            this.Program();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
        return true;
    }

    private void Match(string token)
    {
        if (this.iterator.Current == token) this.iterator.MoveNext();
        else throw new Exception("Expected token mismatch exception.");
    }

    private string GetName()
    {
        var name = this.iterator.Current;
        this.iterator.MoveNext();
        return name;
    }

    private void Program()
    {
        this.iterator.Reset();
        this.iterator.MoveNext();
        this.Procedure();
    }

    private void Procedure()
    {
        this.Match("procedure");
        var procedure_name = this.GetName();
        this.Match("{");
        var statements = this.StmtList(new List<Statement>());
        this.Match("}");
        var procedure = PKBExtensions.CreateProdecure(procedure_name, statements);
        PKBExtensions.AddProcedureToPKBStorage(procedure);
    }

    private List<Statement> StmtList(List<Statement> statements)
    {
        statements.Add(this.Stmt());
        if (iterator.Current == "}") return statements;
        else return StmtList(statements);
    }

    private Statement Stmt()
    {
        switch(this.iterator.Current)
        {
            case "while":
                return this.While();
            default:
                return this.Assign();
        }
    }

    private WhileStatement While()
    {
        this.Match("while");
        var variable_name = this.GetName();
        this.Match("{");
        var statements = this.StmtList(new List<Statement>());
        this.Match("}");
        return PKBExtensions.CreateWhileStatement(variable_name, statements);
    }

    private AssignStatement Assign()
    {
        var variable_name = this.GetName();
        this.Match("=");
        var expression = this.Expr();
        return PKBExtensions.CreateAssignStatement(variable_name, expression);
    }

    private Expression Expr()
    {
        var left = this.GetName();
        Expression leftExpr = ParseExpression(left);

        switch (this.iterator.Current)
        {
            case ";":
                this.Match(";");
                return leftExpr;
            case "+":
                this.Match("+");
                string right = this.GetName();
                Expression rightExpr = ParseExpression(right);
                this.Match(";");
                return PKBExtensions.CreateBinaryExpression(leftExpr, "+", rightExpr);
            default:
                throw new Exception("Error parsing expression.");
        }
    }

    private Expression ParseExpression(string expr)
    {
        if (int.TryParse(expr, out int result))
        {
            return PKBExtensions.CreateConstExpression(result);
        }
        else
        {
            return PKBExtensions.CreateVariableExpression(expr);
        }
    }
}