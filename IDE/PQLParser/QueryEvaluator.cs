using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.SIMPLE;
using Microsoft.Win32;

namespace IDE.PQLParser;

public class QueryEvaluator
{
    private HashSet<String> output;
    private Tuple<String, String> select; //name, nodeType
    private List<Tuple<String, String, String>> with; //synonym, attr, comparable
    private readonly IPKBQuery pkb;

    public QueryEvaluator() 
    {
        output = [];
        select = Tuple.Create("","");
        with = [];
        pkb = new PKBQueryService();
    }

    public String EvaluateQuery(QueryTree tree)
    {
        output = [];
        select = Tuple.Create("","");
        with = [];
        bool isSuchThatPresent = false;
        bool isWithPresent = false;

        foreach (var child in tree.Children)
        {
            if (child.NodeType == "select") // na razie założenie jednego synonimu w select
            {
                select = Tuple.Create(child.Children[0].Name, child.Children[0].NodeType);
                // Console.WriteLine("selects: " + select.ToString());
            }
            if (child.NodeType == "with")
            {
                isWithPresent = true;
                List<String> condition = [];
                foreach (var arg in child.Children)
                {
                    condition.Add(arg.Name);
                    if (condition.Count >= 3)
                    {
                        with.Add(Tuple.Create(condition[0], condition[1], condition[2]));
                        condition.Clear();
                    }
                }
                // sprawdzenie
                // foreach (var w in with)
                // {
                //     Console.WriteLine("with: " + w.ToString());
                // }
            }
        }
        foreach (var child in tree.Children)
        {
            //Console.WriteLine("tree node: " + child.ToString());
            if (child.NodeType == "suchthat")
            {
                HashSet<String> partialOutput = [];
                isSuchThatPresent = true;
                foreach (var relation in child.Children)
                {
                    var arg1 = relation.Children[0];
                    var arg2 = relation.Children[1];
                    switch (relation.Name)
                    {
                        case "Modifies":
                            //Console.WriteLine($"Evaluate Modifies ({arg1.ToString()},{arg2.ToString()})");
                            partialOutput = EvaluateRelation("Modifies", arg1, arg2);
                        break;
                        case "Follows":
                            //Console.WriteLine($"Evaluate Follows ({arg1.ToString()},{arg2.ToString()})");
                            partialOutput = EvaluateRelation("Follows", arg1, arg2);
                        break;
                        case "Follows*":
                            //Console.WriteLine($"Evaluate Follows* ({arg1.ToString()},{arg2.ToString()})");
                            partialOutput = EvaluateRelation("Follows*", arg1, arg2);
                        break;
                        case "Uses":
                            //Console.WriteLine($"Evaluate Uses ({arg1.ToString()},{arg2.ToString()})");
                            partialOutput = EvaluateRelation("Uses", arg1, arg2);
                        break;
                        case "Parent":
                            partialOutput = EvaluateRelation("Parent", arg1, arg2);
                        break;
                        case "Parent*":
                            //Console.WriteLine($"Evaluate Parent* ({arg1.ToString()},{arg2.ToString()})");
                            partialOutput = EvaluateRelation("Parent*", arg1, arg2);
                        break;
                        case "Calls":
                            partialOutput = EvaluateRelation("Calls", arg1, arg2);
                        break;
                        case "Calls*":
                            partialOutput = EvaluateRelation("Calls*", arg1, arg2);
                        break;
                    }
                    if (partialOutput.Count == 0) return "";
                    if (output.Count == 0) output = partialOutput; else output.Intersect(partialOutput);
                }
            }
        }

        if (!isSuchThatPresent)
        {
            EvaluateSelectWithoutSuchThat();
        }
        if (isWithPresent)
        {
            EvaluateWithStatementsOnSelect(tree);
        }

        return buildStringResponse(output);
    }

    private void EvaluateSelectWithoutSuchThat()
    {
        // Console.WriteLine("EvaluateSelectWithoutSuchThat");
        if (select.Item2 == "Procedure")
        {
            IEnumerable<String> allProcedures = pkb.GetAllProcedureNames();
            foreach (var procedure in allProcedures)
            {
                output.Add(procedure);
            }
        }
    }
    private HashSet<string> EvaluateRelation(String relation, QueryTree arg1, QueryTree arg2)
    {
        HashSet<String> result = [];
        bool isNotConditioned = true;
        if (select.Item1 == arg1.Name)
        {
            if (arg2.NodeType == "Number")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<int>(relation, false, Int32.Parse(arg2.Name));
            }
            if (arg2.NodeType == "String")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<String>(relation, false, arg2.Name);                
            }
            //Console.WriteLine("select arg1");
            if (isNotConditioned)
            {
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
                                    result.Add(line.ToString());
                                }
                                // Console.WriteLine($"invoked GetStatementsModyfing({modifiedValName})");
                            break;
                            case "Follows":
                                int followedStmt = Int16.Parse(condition.Item3);
                                int? followingStatement = pkb.GetFollows(followedStmt);
                                if (followingStatement != null) result.Add(followingStatement.ToString());
                                // Console.WriteLine($"invoked pkb.GetFollows({followedStmt})");
                            break;
                            case "Follows*":
                                int followedTStmt = Int16.Parse(condition.Item3);
                                IEnumerable<int> followingStatements = pkb.GetAllFollowingStatements(followedTStmt);
                                foreach (var stmt in followingStatements)
                                {
                                    result.Add(stmt.ToString());
                                }
                                // Console.WriteLine($"invoked pkb.GetAllFollowingStatements({followedTStmt})");
                            break;
                            case "Uses":
                                string usedValName = condition.Item3;
                                IEnumerable<int> usingStatements = pkb.GetStatementsUsing(usedValName);
                                foreach (var stmt in usingStatements)
                                {
                                    result.Add(stmt.ToString());
                                }
                                // Console.WriteLine($"invoked GetStatementsUsing({usedValName})");
                            break;
                            case "Parent":
                                int childStmt = Int16.Parse(condition.Item3);
                                int? parentStmt = pkb.GetParent(childStmt);
                                if (parentStmt != null) result.Add(parentStmt.ToString());
                                // Console.WriteLine($"invoked pkb.GetParent({childStmt})");           
                            break;
                            case "Parent*":
                                int childTStmt = Int16.Parse(condition.Item3);
                                IEnumerable<int> parentStmts = pkb.GetAllParentStatements(childTStmt);
                                foreach (var stmt in parentStmts)
                                {
                                    result.Add(stmt.ToString());
                                }
                                // Console.WriteLine($"invoked pkb.GetAllParentStatements({childTStmt})");  
                            break;
                            case "Calls":
                                String callee = condition.Item3;
                                IEnumerable<String> procedures = pkb.GetCallingProcedures(callee);
                                foreach (var procedure in procedures)
                                {
                                    result.Add(procedure);
                                }
                                // Console.WriteLine($"invoked pkb.GetCallingProcedures({callee})");
                            break;
                            case "Calls*":
                                //TODO extension
                            break;
                        }
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
                            result.Add(stmt.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllStatementsModyfingAnything()");
                    break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followingStatements = pkb.GetAllFollowsTargets();
                        foreach (var stmt in followingStatements)
                        {
                            result.Add(stmt.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllFollowsSources()");
                    break;
                    case "Uses":
                        IEnumerable<int> usingAnythingStmts = pkb.GetAllStatementsUsingAnything();
                        foreach (var stmt in usingAnythingStmts)
                        {
                            result.Add(stmt.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllStatementsUsingAnything()");
                    break;
                    case "Parent":
                    case "Parent*":
                        IEnumerable<int> allParents = pkb.GetAllParentStatements();
                        foreach (var parent in allParents)
                        {
                            result.Add(parent.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllParentStatements()");
                    break;
                    case "Calls":
    //TODO ext
                    break;
                    case "Calls*":
    //TODO ext
                    break;
                }
            }
        }
        if (select.Item1 == arg2.Name)
        {
            if (arg1.NodeType == "Number")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<int>(relation, true, Int32.Parse(arg1.Name));
            }
            if (arg1.NodeType == "String")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<String>(relation, true, arg1.Name);                
            }
            //Console.WriteLine("select arg2");
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
                                result.Add(variable);
                            }
                            // Console.WriteLine($"invoked GetModifiedVariables({modyfyingStmtNumber})");
                        break;
                        case "Follows":
                            int followingStatement = Int16.Parse(condition.Item3);
                            int? followedStatement = pkb.GetFollowedBy(followingStatement);
                            if (followedStatement != null)
                            {
                                result.Add(followedStatement.ToString());
                            }
                            // Console.WriteLine($"invoked pkb.GetFollowedBy({followingStatement})");
                        break;
                        case "Follows*":
                            int followingTStatement = Int16.Parse(condition.Item3);
                            IEnumerable<int> followedTStaments = pkb.GetAllStatementsLeadingTo(followingTStatement);
                            foreach (var stmt in followedTStaments)
                            {
                                result.Add(stmt.ToString());
                            }
                            // Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({followingTStatement})");
                        break;
                        case "Uses":
                            int usingStmtNumber = Int16.Parse(condition.Item3);
                            IEnumerable<String> usedVariables = pkb.GetUsedVariables(usingStmtNumber);
                            foreach (var variable in usedVariables)
                            {
                                result.Add(variable);
                            }
                            // Console.WriteLine($"invoked GetUsedVariables({usingStmtNumber})");
                        break;
                        case "Parent":
                            int parentStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> childrenStmt = pkb.GetChildren(parentStmt);
                            foreach (var stmt in childrenStmt)
                            {
                                result.Add(stmt.ToString());
                            }
                            // Console.WriteLine($"invoked pkb.GetChildren({parentStmt})");
                        break;
                        case "Parent*":
                            int parentTStmt = Int16.Parse(condition.Item3);
                            IEnumerable<int> childStmts = pkb.GetAllNestedStatements(parentTStmt);
                            foreach (var stmt in childStmts)
                            {
                                result.Add(stmt.ToString());
                            }
                            // Console.WriteLine($"invoked pkb.GetAllNestedStatements({parentTStmt})");
                        break;
                        case "Calls":
                            String caller = condition.Item3;
                            IEnumerable<String> procedures = pkb.GetCallingProcedures(caller);
                            foreach (var procedure in procedures)
                            {
                                result.Add(procedure);
                            }
                            // Console.WriteLine($"invoked pkb.GetCallingProcedures({caller})");  
                        break;
                        case "Calls*":
//TODO ext
                        break;
                    }
                }
            }
            if (isNotConditioned)
            {          
                switch (relation)
                {
                    case "Modifies":
                        IEnumerable<string> allModifiedVariables = pkb.GetAllModifiedVariables();
                        foreach (var variable in allModifiedVariables)
                        {
                            result.Add(variable);
                        }
                        // Console.WriteLine("invoked pkb.GetAllModifiedVariables()");
                    break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followedStatements = pkb.GetAllFollowsSources();
                        foreach (var stmt in followedStatements)
                        {
                            result.Add(stmt.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllFollowsTargets()");
                    break;
                    case "Uses":
                        IEnumerable<string> allUsedVariables = pkb.GetAllUsedVariables();
                        foreach (var variable in allUsedVariables)
                        {
                            result.Add(variable);
                        }
                        // Console.WriteLine("invoked GetAllUsedVariables()");
                    break;
                    case "Parent":
                    case "Parent*":
                        IEnumerable<int> allChildren = pkb.GetAllChildStatements();
                        foreach (var child in allChildren)
                        {
                            result.Add(child.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllChildStatements()");
                    break;
                    case "Calls":
                        //TODO ext
                    break;
                    case "Calls*":
                        //TODO ext
                    break;
                }
            }
        }
        return result;
    }
    private void EvaluateWithStatementsOnSelect(QueryTree tree)
    {
        foreach (var child in tree.Children) 
        {
            if (child.NodeType == "with")
            {
                var conditionsTable = child.Children;
                for (var arg = 0; arg < conditionsTable.Count; arg += 3)
                {
                    if (select.Item1 == conditionsTable[arg].Name) // z założeniem ze jest tylko jeden select
                    {
                        output.RemoveWhere(s => s != conditionsTable[arg + 2].Name);
                    }
                }
            }
        }
    }

    private HashSet<String> EvaluateRelationWithParameter<T>(String relation, bool isFirstArgParameter, T parameter)
    {
        HashSet<String> result = [];
        if (parameter is int intParameter)
        {
            switch(relation)
            {
                case "Follows":
                    if (isFirstArgParameter)
                    {
                        int? followed = pkb.GetFollowedBy(intParameter);
                        if (followed != null) result.Add(followed.ToString());
                        // Console.WriteLine($"invoked pkb.GetFollowedBy({intParameter})"); 
                    } 
                    else
                    {
                        int? follower = pkb.GetFollows(intParameter);
                        if (follower != null) result.Add(follower.ToString());
                        // Console.WriteLine($"invoked pkb.GetFollows({intParameter})");   
                    }
                break;
                case "Follows*":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<int> targets = pkb.GetAllFollowingStatements(intParameter);
                        foreach (var target in targets)
                        {
                            result.Add(target.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetAllFollowingStatements({intParameter})"); 
                    } 
                    else
                    {
                        IEnumerable<int> precedents = pkb.GetAllStatementsLeadingTo(intParameter);
                        foreach (var precedent in precedents)
                        {
                            result.Add(precedent.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({intParameter})");   
                    }
                break;
                case "Uses":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<string> variables = pkb.GetUsedVariables(intParameter);
                        foreach (var variable in variables)
                        {
                            result.Add(variable.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetUsedVariables({intParameter})"); 
                    } 
                break;
                case "Modifies":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<string> modified = pkb.GetModifiedVariables(intParameter);
                        foreach (var mod in modified)
                        {
                            result.Add(mod.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetModifiedVariables({intParameter})"); 
                    }       
                break;
                case "Parent":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<int> children = pkb.GetChildren(intParameter);
                        foreach (var child in children)
                        {
                            result.Add(child.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetChildren({intParameter})"); 
                    } 
                    else
                    {
                        int? parent = pkb.GetParent(intParameter);
                        if (parent != null) result.Add(parent.ToString());
                        // Console.WriteLine($"invoked pkb.GetParent({intParameter})");   
                    }      
                break;
                case "Parent*":
                    IEnumerable<int> parentStmts = pkb.GetAllParentStatements(intParameter);
                    foreach (var stmt in parentStmts)
                    {
                        result.Add(stmt.ToString());
                    }
                    // Console.WriteLine($"invoked pkb.GetAllParentStatements({intParameter})");  
                break;
            }
        }
        if (parameter is String stringParameter)
        {
            switch (relation)
            {
                case "Modifies":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<string> variables = pkb.GetModifiedVariables(stringParameter);
                        foreach (var variable in variables)
                        {
                            result.Add(variable.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetModifiedVariables({stringParameter})"); 
                    }
                    else 
                    {
                        IEnumerable<string> procedures = pkb.GetProceduresModifying(stringParameter);
                        foreach (var procedure in procedures)
                        {
                            result.Add(procedure.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetProceduresModifying({stringParameter})");
                    }     
                break;
                case "Uses":
                    if (!isFirstArgParameter)
                    {
                        IEnumerable<int> variables = pkb.GetStatementsUsing(stringParameter);
                        foreach (var variable in variables)
                        {
                            result.Add(variable.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetStatementsUsing({stringParameter})"); 
                    }
                break;
                case "Calls":
                    if (isFirstArgParameter)
                    {
                        IEnumerable<String> procedures = pkb.GetCallingProcedures(stringParameter);
                        foreach (var procedure in procedures)
                        {
                            result.Add(procedure.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetCallingProcedures({stringParameter})"); 
                    }
                    else
                    {
                        IEnumerable<String> procedures = pkb.GetCalledProcedures(stringParameter);
                        foreach (var procedure in procedures)
                        {
                            result.Add(procedure.ToString());
                        }
                        // Console.WriteLine($"invoked pkb.GetCalledProcedures({stringParameter})"); 
                    }
                break;
                case "Calls*":
                    //TODO ext
                break;
            }
        }
        return result;
    }

    private String buildStringResponse(HashSet<String> outputSet)
    {
        if (outputSet.Count == 0) return "";
        String stringOutput = outputSet.First();
        foreach (var element in output.Skip(1))
        {
            stringOutput += ", " + element;
        }
        return stringOutput;
    }
}
