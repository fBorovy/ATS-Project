using System;
using System.Xml.XPath;

namespace IDE.PQLParser;

public class QueryPreprocessor
{
    private List<QueryKeyword> _currentQuery;
    private int _currentKeyword;
    private HashSet<Synonym> _declaredSynonyms;
    private QueryKeyword CurrentQueryKeyword => _currentQuery[_currentKeyword];
    private QueryKeyword Advance() => _currentQuery[_currentKeyword++];

    public QueryPreprocessor() {
        _currentQuery = new List<QueryKeyword>();
        _currentKeyword = 0;
        _declaredSynonyms = new ();
    }

    public QueryTree BuildQueryTree(List<QueryKeyword> currentQuery) 
    {
        _currentQuery = currentQuery;
        _currentKeyword = 0;
        QueryTree tree = new QueryTree("root", "root");
        bool eof = false;

        ParseSynonymDeclarations();
        tree.AddNode(ParseSelect());
        if (Match(QueryKeywordType.SuchThat))
        {
            tree.AddNode(ParseSuchThat());
        }
        if (Match(QueryKeywordType.With)) 
        {
            tree.AddNode(ParseWith());
        }

        //Console.WriteLine("____________________________");
        // while (!eof) 
        // {
        //     if (Match(QueryKeywordType.End))
        //     {
        //         eof = true;
        //         Console.WriteLine("Parsed eof");
        //     }
        //     else
        //     {
        //         Console.WriteLine($"Parsed other: {CurrentQueryKeyword.Value}");
        //         Advance();
        //     }
        // }
        _currentKeyword = 0;
        return tree;
    }

    public void ValidateQuery(List<QueryKeyword> currentQuery)
    {
        _currentQuery = currentQuery;
        _currentKeyword = 0;
        _declaredSynonyms = new();

        ValidateSynonymDeclarations();
    }

    private void ValidateSynonymDeclarations()
    {
        while (!Match(QueryKeywordType.Select)) 
        {
            DeclareSynonymsOrThrowException();
            ExpectKeyword(QueryKeywordType.Identifier);

        }
        //sprawdzenie
        //Console.WriteLine("Synonyms declarations valid.");
    }

    private void DeclareSynonymsOrThrowException()
    {
        while (!Match(QueryKeywordType.Select))
        {
            if (SynonymTypeResolver.TryParse(CurrentQueryKeyword.Value, out var synonymType))
            {
                Advance();
                //Console.WriteLine($"synonym type: {synonymType}");
                InsertSynonymsOfAType(synonymType);   
            }
            else throw new SynonymException($"Design entity expected, got {CurrentQueryKeyword.Value}");
        }
        
    }

    private void ParseSynonymDeclarations()
    {
        while (!Match(QueryKeywordType.Select)) 
        {
            if (SynonymTypeResolver.TryParse(CurrentQueryKeyword.Value, out var synonymType))
            {
                Advance();
                //Console.WriteLine($"synonym type: {synonymType}");
                InsertSynonymsOfAType(synonymType);
                
            }
        //Console.WriteLine($"Last identifier: {CurrentQueryKeyword.Value}");
        }
        //sprawdzenie
        // foreach (var synonym in _declaredSynonyms)
        // {
        //     Console.WriteLine("parsed synonym" + synonym.Type + " " + synonym.Name);
        // }
    }
    private QueryTree ParseWith()
    {
        QueryTree withNode = new QueryTree("with", "with");
        //Console.WriteLine($"parsed: {CurrentQueryKeyword.Value}");
        Advance();
        do {
            if (Match(QueryKeywordType.And)) Advance();
            if (Match(QueryKeywordType.Identifier))
            {
                Synonym? synonym = GetDeclaredSynonym(CurrentQueryKeyword.Value);
                if (synonym != null) 
                {
                    //Console.WriteLine($"Parsed synonym identifier: {CurrentQueryKeyword.Value}");
                    withNode.AddNode(new QueryTree(synonym.Value.Name, "synonym"));
                    Advance();
                    if (Match(QueryKeywordType.Attribute))
                    {
                        withNode.AddNode(new QueryTree(CurrentQueryKeyword.Value, "attr"));
                        Advance();
                    }
                }
                if (Match(QueryKeywordType.Equals))
                {
                    //Console.WriteLine($"Parsed equation: {CurrentQueryKeyword.Value}");
                    Advance();
                }
                if (Match(QueryKeywordType.String) || Match(QueryKeywordType.Number) || Match(QueryKeywordType.Identifier)) {
                    withNode.AddNode(new QueryTree(CurrentQueryKeyword.Value, "comparable"));
                    if (Match(QueryKeywordType.Identifier)) Advance();
                    Advance();
                }
            }
        }
        while (Match(QueryKeywordType.And));
        // sprawdzenie
        // Console.WriteLine($"node: {withNode.ToString()}");
        // foreach (var node in withNode.Children)
        // {
        //     Console.WriteLine($"node: {node.ToString()}");
        // }

        return withNode;
    }

    private QueryTree ParseSelect()
    {
        QueryTree selectNode = new QueryTree("select", "select");
        //Console.WriteLine($"parsed: {CurrentQueryKeyword.Value}");
        Advance();
        while (!Match(QueryKeywordType.SuchThat))
        {
            if (Match(QueryKeywordType.Identifier))
            {
                Synonym? synonym = GetDeclaredSynonym(CurrentQueryKeyword.Value);
                if (synonym != null) 
                {
                    //Console.WriteLine($"Parsed synonym identifier: {CurrentQueryKeyword.Value}");
                    selectNode.AddNode(new QueryTree(synonym.Value.Name, synonym.Value.Type.ToString()));
                    Advance();
                }
            }
            if (Match(QueryKeywordType.Comma))
            {
                //Console.WriteLine($"Parsed comma: {CurrentQueryKeyword.Value}");
                Advance();
            }
        }
        //sprawdzenie
        // Console.WriteLine($"QueryTree node: {selectNode}");
        // foreach (var node in selectNode.Children)
        // {
        //     Console.WriteLine($"QueryTree node: {node}");
        // }
        return selectNode;
    }


    private QueryTree ParseSuchThat()
    {
        QueryTree suchThatNode = new QueryTree("suchthat", "suchthat");
        Advance();
        //Console.WriteLine($"parsed suchthat: {CurrentQueryKeyword.Value}");
        do
        {
            //Console.WriteLine("relations loop");
            if(Match(QueryKeywordType.And))
            {
                Advance();
            }
            suchThatNode.AddNode(ParseRelation());
        }
        while (Match(QueryKeywordType.And));

        //sprawdzenie
        //Console.WriteLine($"QueryTree node: {suchThatNode.ToString()}");
        // foreach (var symbol in suchThatNode.Children)
        // {
        //     Console.WriteLine($"QueryTree node: {symbol.ToString()}");
        //     foreach (var arg in symbol.Children)
        //     {
        //         Console.WriteLine($"{symbol.Name} node child: {arg.ToString()}");
        //     }
        // }
        return suchThatNode;
    }
    private QueryTree ParseRelation()
    {
        QueryTree relationNode = new QueryTree(CurrentQueryKeyword.Value, "relation");
        Advance();
        if (Match(QueryKeywordType.OpenParen)) 
        {
            //Console.WriteLine($"Parsed open paren: {CurrentQueryKeyword.Value}");
            Advance();
        }
        if (Match(QueryKeywordType.Identifier, QueryKeywordType.Joker, QueryKeywordType.Number))
        {
            //Console.WriteLine($"matchidentifier: {CurrentQueryKeyword.Value}");
            Synonym? arg1 = GetDeclaredSynonym(CurrentQueryKeyword.Value);
            if (arg1 != null)
            {
                relationNode.AddNode(new QueryTree(arg1.Value.Name, arg1.Value.Type.ToString()));
            }
            else if (CurrentQueryKeyword.Type == QueryKeywordType.Joker || CurrentQueryKeyword.Type == QueryKeywordType.Number) 
            {
                relationNode.AddNode(new QueryTree(CurrentQueryKeyword.Value, CurrentQueryKeyword.Type.ToString()));
            }
            Advance();
        }
        if (Match(QueryKeywordType.Comma)) Advance();
        if (Match(QueryKeywordType.Identifier, QueryKeywordType.Joker, QueryKeywordType.String, QueryKeywordType.Number))
        {
            Synonym? arg2 = GetDeclaredSynonym(CurrentQueryKeyword.Value);
            if (arg2 != null)
            {
                relationNode.AddNode(new QueryTree(arg2.Value.Name, arg2.Value.Type.ToString()));
            } 
            else if (CurrentQueryKeyword.Type == QueryKeywordType.Joker || CurrentQueryKeyword.Type == QueryKeywordType.String || CurrentQueryKeyword.Type == QueryKeywordType.Number) 
            {
                relationNode.AddNode(new QueryTree(CurrentQueryKeyword.Value, CurrentQueryKeyword.Type.ToString()));
            }
            Advance();
        }
        if (Match(QueryKeywordType.CloseParen))
        {
            //Console.WriteLine($"Parsed close paren: {CurrentQueryKeyword.Value}");
            Advance();
        }
        return relationNode;
    }

    private void InsertSynonymsOfAType(SynonymType type)
    {
        while (Match(QueryKeywordType.Identifier))
        {
            //Console.WriteLine($"New identifier: {CurrentQueryKeyword.Value} {type}");
            _declaredSynonyms.Add(new Synonym(type, CurrentQueryKeyword.Value));
            Advance();
            if (Match(QueryKeywordType.Comma)) Advance();
        } 
    }

    private bool Match(params QueryKeywordType[] types)
    {
        foreach (var type in types)
        {
            if (CurrentQueryKeyword.Type == type)
                return true;
        }
        return false;
    }
 
    private void ExpectKeyword(params QueryKeywordType[] types)
    {
        if (!Match(types)) 
            throw new Exception($"Unexpected token: {CurrentQueryKeyword.Value}");
        else 
            Advance();
    }

    private Synonym? GetDeclaredSynonym(string synonymName)
    {
        foreach (Synonym synonym in _declaredSynonyms)
        {
            if (synonym.Name == synonymName) return synonym;
        }
        return null;
    }

    private void ExpectDesignEntity()
    {
        if (!Match(
            QueryKeywordType.Statement,
            QueryKeywordType.Assign,
            QueryKeywordType.Constant,
            QueryKeywordType.Procedure,
            QueryKeywordType.Prog_line,
            QueryKeywordType.Variable,
            QueryKeywordType.While
        )) throw new SynonymException($"Invalid design entity {CurrentQueryKeyword.Value}");
    }
}