using System.Text.RegularExpressions;

using Atsi.Domain.Extensions;
using Atsi.Structures.PKB;
using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Statements;
using Atsi.Structures.Utils.Enums;

namespace IDE.Parser;

public class CodeParser
{
    private string path;
    private List<string> words;
    private int iterator;
    private string currentProcedure;

    public CodeParser(string path)
    {
        this.path = path;
    }

    public bool ReadFile()
    {
        string? content = File.ReadAllText(this.path);
        if (content == null) return false;
        this.words = Regex
            .Split(content, @"(?<=\S)(?=\s)|(?<=\s)(?=\S)|(?<=[+\-*/={};])|(?=[+\-*/={};])")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToList();
        return this.words.Any();
    }

    public bool Parse()
    {
        try
        {
            this.iterator = 0;
            this.currentProcedure = string.Empty;
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
        if (this.words[this.iterator] == token) this.iterator++;
        else throw new Exception($"Expected token mismatch exception. Expected {token}, received {this.words[this.iterator]}");
    }

    private string GetName()
    {
        var name = this.words[this.iterator];
        this.iterator++;
        return name;
    }

    private void Program()
    {
        while (this.iterator < this.words.Count)
            this.Procedure();
    }

    private void Procedure()
    {
        this.Match("procedure");
        this.currentProcedure = this.GetName();
        this.Match("{");
        var statements = new List<Statement>();
        while (this.words[this.iterator] != "}")
        {
            statements.Add(this.Stmt());
        }
        this.Match("}");
        var procedure = PKBExtensions.CreateProcedure(this.currentProcedure, statements);
        PKBExtensions.AddProcedureToPKBStorage(procedure);
    }

    private Statement Stmt()
    {
        switch(this.words[this.iterator])
        {
            case "while":
                return this.While();
            case "if":
                return this.If();
            case "call":
                return this.Call();
            default:
                return this.Assign();
        }
    }

    private WhileStatement While()
    {
        this.Match("while");
        var variable_name = this.GetName();
        var while_statement = PKBExtensions.CreateWhileStatement(this.currentProcedure, variable_name);
        this.Match("{");
        var statements = new List<Statement>();
        while (this.words[this.iterator] != "}")
        {
            statements.Add(this.Stmt());
        }
        this.Match("}");
        return PKBExtensions.UpdateWhileStatement(while_statement, statements);
    }

    private IfStatement If()
    {
        this.Match("if");
        var variable_name = this.GetName();
        var if_statement = PKBExtensions.CreateIfStatement(this.currentProcedure, variable_name);
        this.Match("then");
        this.Match("{");
        var then_statements = new List<Statement>();
        while (this.words[this.iterator] != "}")
        {
            then_statements.Add(this.Stmt());
        }
        this.Match("}");
        this.Match("else");
        this.Match("{");
        var else_statements = new List<Statement>();
        while (this.words[this.iterator] != "}")
        {
            else_statements.Add(this.Stmt());
        }
        this.Match("}");
        return PKBExtensions.UpdateIfStatement(if_statement, then_statements, else_statements);
    }

    private CallStatement Call()
    {
        this.Match("call");
        var called_procedure_name = this.GetName();
        this.Match(";");
        return PKBExtensions.CreateCallStatement(this.currentProcedure, called_procedure_name);
    }

    private AssignStatement Assign()
    {
        var variable_name = this.GetName();
        this.Match("=");
        var expression = this.Expr();
        return PKBExtensions.CreateAssignStatement(this.currentProcedure, variable_name, expression);
    }

    private Expression Expr()
    {
        var left = this.GetName();
        Expression left_expr = ParseExpression(left);
        string symbol = this.words[this.iterator];
        while (symbol != ";")
        {
            this.Match(symbol);
            string right = this.GetName();
            var right_expr = ParseExpression(right);
            DictAvailableArythmeticSymbols op = symbol switch
            {
                "+" => DictAvailableArythmeticSymbols.Plus,
                "-" => DictAvailableArythmeticSymbols.Minus,
                "*" => DictAvailableArythmeticSymbols.Times,
                "/" => DictAvailableArythmeticSymbols.Divide,
                _ => throw new Exception($"Unknown operator: {symbol}"),
            };
            left_expr = PKBExtensions.CreateBinaryExpression(left_expr, op, right_expr);
            symbol = this.words[this.iterator];
        }
        this.Match(";");
        return left_expr;
    }

    private Expression ParseExpression(string expr)
    {
        if (int.TryParse(expr, out int result))
            return PKBExtensions.CreateConstExpression(result);
        else
            return PKBExtensions.CreateVariableExpression(expr);
    }
}