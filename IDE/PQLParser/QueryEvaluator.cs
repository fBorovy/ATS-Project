using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Atsi.Structures.PKB.Explorer;
using Microsoft.Win32;

namespace IDE.PQLParser;

public class QueryEvaluator
{
    private String output;
    private Tuple<String, String> select; //name, nodeType
    private List<Tuple<String, String, String>> with; //synonym, attr, comparable
    private readonly IPKBQuery pkb;

    public QueryEvaluator() 
    {
        output = "";
        select = Tuple.Create("","");
        with = [];
        pkb = new PKBQueryService();
    }

    public String EvaluateQuery(QueryTree tree)
    {
        output = "";
        select = Tuple.Create("","");
        with = [];
        Console.WriteLine("evaluation__________________");

        foreach (var child in tree.Children)
        {
            if (child.NodeType == "select") // na razie założenie jednego synonimu w select
            {
                select = Tuple.Create(child.Children[0].Name, child.Children[0].NodeType);
                Console.WriteLine("selects: " + select.ToString());
            }
            if (child.NodeType == "with")
            {
                List<String> condition = [];
                foreach (var arg in child.Children)
                {
                    condition.Add(arg.Name);
                    if (condition.Count >= 3)
                    {
                        with.Add(Tuple.Create(condition[0], condition[1], condition[2]));
                    }
                }
                foreach (var w in with)
                {
                    Console.WriteLine("with: " + w.ToString());
                }
            }
            
        }
        foreach (var child in tree.Children)
        {
            Console.WriteLine("tree node: " + child.ToString());
            if (child.NodeType == "suchthat")
            {
                foreach (var relation in child.Children)
                {
                    var arg1 = relation.Children[0];
                    var arg2 = relation.Children[1];
                    switch (relation.Name)
                    {
                        case "Modifies":
                            Console.WriteLine($"Evaluate Modifies ({arg1.ToString()},{arg2.ToString()})");
                            output += EvaluateRelation("Modifies", arg1, arg2);
                        break;
                        case "Follows":
                            Console.WriteLine($"Evaluate Follows ({arg1.ToString()},{arg2.ToString()})");
                            output += EvaluateRelation("Follows", arg1, arg2);
                        break;
                        case "Follows*":
                            Console.WriteLine($"Evaluate Follows* ({arg1.ToString()},{arg2.ToString()})");
                            output += EvaluateRelation("Follows*", arg1, arg2);    
                        break;
                        case "Uses":
                            Console.WriteLine($"Evaluate Uses ({arg1.ToString()},{arg2.ToString()})");
                            output += EvaluateRelation("Uses", arg1, arg2);
                        break;
                        case "Parent":
                            Console.WriteLine($"Evaluate Parent ({arg1.ToString()},{arg2.ToString()})");
                            output += EvaluateRelation("Parent", arg1, arg2);
                        break;
                    }
                }
            }
        }

        return output;
    }

    private string EvaluateRelation(String relation, QueryTree arg1, QueryTree arg2)
    {
        String result = "";
        bool isNotConditioned = true;
        if (select.Item1 == arg1.Name)
        {
            Console.WriteLine("select arg1");
            foreach (var condition in with)
            {
                if (condition.Item1 == arg2.Name)
                {
                    isNotConditioned = false;
                    switch (relation)
                    {
                        case "Modifies":
                            string modifiedValName = condition.Item3;
                            IEnumerable<int> modifyingStatements = pkb.GetStatementsModifying(modifiedValName);
                            foreach (var line in modifyingStatements)
                            {
                                result += line.ToString() + ",";
                            }
                            Console.WriteLine($"invoked GetStatementsModyfing({modifiedValName})");
                        break;
                        case "Follows":
                            int followedStmt = Int16.Parse(condition.Item3);
                            int? followingStatement = pkb.GetFollows(followedStmt);
                            if (followingStatement != null) result += followingStatement;
                            Console.WriteLine($"invoked pkb.GetFollows({followedStmt})");
                        break;
                        case "Follows*":
                            int followedTStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> followingStatements = pkb.GetAllFollowingStatements(followedStmt);
                            foreach (var stmt in followingStatements)
                            {
                                result += stmt.ToString() + ",";
                            }
                            Console.WriteLine($"invoked pkb.GetAllFollowingStatements({followedTStmt})");
                        break;
                        case "Uses":
                            string usedValName = condition.Item3;
                            IEnumerable<int> usingStatements = pkb.GetStatementsUsing(usedValName);
                            foreach (var stmt in usingStatements)
                            {
                                result += stmt.ToString() + ",";
                            }
                            Console.WriteLine($"invoked GetStatementsUsing({usedValName})");
                        break;
                        case "Parent":
                            int childStmt = Int16.Parse(condition.Item3);
                            int? parentStmt = pkb.GetParent(childStmt);
                            if (parentStmt != null) result += parentStmt;
                            Console.WriteLine($"invoked pkb.GetParent({childStmt})");           
                        break;
                        case "Parent*":
                            int childTStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> parentStmts = pkb.GetAllParentStatements(childStmt);
                            foreach (var stmt in parentStmts)
                            {
                                result += stmt.ToString() + ",";
                            }
                            Console.WriteLine($"invoked pkb.GetAllParentStatements({childStmt})");  
                        break;
                    }
                }
            }
            if (isNotConditioned)
            {
                switch (relation)
                {
                case "Modifies":
                    IEnumerable<int> allModyfyingStmts = pkb.GetAllStatementsModifyingAnything();
                    foreach (var stmt in allModyfyingStmts)
                    {
                        result += stmt.ToString() + ",";
                    }
                    Console.WriteLine("invoked pkb.GetAllStatementsModyfingAnything()");
                break;
                case "Follows":
                case "Follows*":
                    IEnumerable<int> followedStatements = pkb.GetAllFollowsSources();
                    foreach (var stmt in followedStatements)
                    {
                        result += stmt.ToString() + ",";
                    }
                    Console.WriteLine("invoked pkb.GetAllFollowsSources()");
                break;
                case "Uses":
                    // TODO
                    Console.WriteLine("invoked GetStatementsUsing()");
                break;
                case "Parent":
                case "Parent*":
                    // TODO
                    Console.WriteLine("invoked pkb.GetParent()");
                break;
                }
            }
        }
        if (select.Item1 == arg2.Name)
        {
            Console.WriteLine("select arg2");
            foreach (var condition in with)
            {
                if (condition.Item1 == arg1.Name)
                {
                    isNotConditioned = false;
                    switch (relation)
                    {
                        case "Modifies":
                            int modyfyingStmtNumber = Int16.Parse(condition.Item3);
                            IEnumerable<String> modifiedVariables = pkb.GetModifiedVariables(modyfyingStmtNumber);
                            foreach (var variable in modifiedVariables)
                            {
                                result += variable + ",";
                            }
                            Console.WriteLine($"invoked GetModifiedVariables({modyfyingStmtNumber})");
                        break;
                        case "Follows":
                            int followingStatement = Int16.Parse(condition.Item3);
                            int? followedStatement = pkb.GetFollowedBy(followingStatement);
                            if (followedStatement != null)
                            {
                                result += followedStatement;
                            }
                            Console.WriteLine($"invoked pkb.GetFollowedBy({followingStatement})");
                        break;
                        case "Follows*":
                            int followingTStatement = Int16.Parse(condition.Item3);
                            IEnumerable<int> followedTStaments = pkb.GetAllStatementsLeadingTo(followingTStatement);
                            foreach (var stmt in followedTStaments)
                            {
                                result += stmt + ",";
                            }
                            Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({followingTStatement})");
                        break;
                        case "Uses":
                            int usingStmtNumber = Int16.Parse(condition.Item3);
                            IEnumerable<String> usedVariables = pkb.GetUsedVariables(usingStmtNumber);
                            foreach (var variable in usedVariables)
                            {
                                result += variable + ",";
                            }
                            Console.WriteLine($"invoked GetUsedVariables({usingStmtNumber})");
                        break;
                        case "Parent":
                            int parentStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> childrenStmt = pkb.GetChildren(parentStmt);
                            foreach (var stmt in childrenStmt)
                            {
                                result += stmt + ",";
                            }
                            Console.WriteLine($"invoked pkb.GetChildren({parentStmt})");
                        break;
                        case "Parent*":
                            int parentTStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> childStmts = pkb.GetAllNestedStatements(parentTStmt);
                            foreach (var stmt in childStmts)
                            {
                                result += stmt + ",";
                            }
                            Console.WriteLine($"invoked pkb.GetAllNestedStatements({parentTStmt})");
                        break;
                    }
                }
            }
            if (isNotConditioned)
            {          
                switch (relation)
                {
                    case "Modifies":
                        IEnumerable<string> allModifiedVariables = GetAllModifiedVariables();
                        foreach (var variable in allModifiedVariables)
                        {
                            result += variable + ",";
                        }
                        Console.WriteLine("invoked pkb.GetAllModifiedVariables()");
                    break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followingStatements = pkb.GetAllFollowsTargets();
                        foreach (var stmt in followingStatements)
                        {
                            result += stmt + ",";
                        }
                        Console.WriteLine("invoked pkb.GetAllFollowsTargets()");
                    break;
                    case "Uses":
                        // TODO
                        Console.WriteLine("invoked GetUsedVariables()");
                    break;
                    case "Parent":
                    case "Parent*":
                        Console.WriteLine("invoked pkb.GetChildren()");
                        // TODO
                    break;
                }
            }
        }
        return result;
    }
}
