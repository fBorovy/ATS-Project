using System.Text.RegularExpressions;

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
        this.words = Regex.Split(content, @"\s+").ToList();
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
        string name = this.iterator.Current;
        this.iterator.MoveNext();
        return name;
    }

    private void Program()
    {
        this.iterator.Reset();
        this.Procedure();
    }

    private void Procedure()
    {
        this.Match("procedure");
        string procedure_name = this.GetName();
        this.Match("{");
        this.StmtList();
        this.Match("}");
    }

    private void StmtList()
    {
        this.Stmt();
        if (iterator.Current == "}") return;
        else StmtList();
    }

    private void Stmt()
    {
        switch(this.iterator.Current)
        {
            case "while":
                this.While();
                break;
            default:
                this.Assign();
                break;
        }
    }

    private void While()
    {
        this.Match("while");
        string variable_name = this.GetName();
        this.Match("{");
        this.StmtList();
        this.Match("}");
    }

    private void Assign()
    {
        string variable_name = this.GetName();
        this.Match("=");
        this.Expr();
    }

    private void Expr()
    {
        string variable_name = this.GetName();
        if (this.iterator.Current == ";") return;
        else if (this.iterator.Current == "+") Expr();
        else throw new Exception("Error parsing expression.");
    }
}