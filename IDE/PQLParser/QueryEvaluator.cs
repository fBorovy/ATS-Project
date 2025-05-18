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
    private List<Tuple<String, String, String, String, String, String>> with; //synonym, attr, comparable
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
                    condition.Add(arg.NodeType);
                    if (condition.Count >= 6)
                    {
                        with.Add(Tuple.Create(condition[0], condition[1], condition[2], condition[3],condition[4],condition[5]));
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
                    if (partialOutput.Count == 0)
                    {
                        if (select.Item1 == "Boolean")
                        {
                            return "FALSE";
                        }
                        else return "";
                    }
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
        if (select.Item1 == "Boolean")
        {
            if (output.Count > 0) { return "TRUE"; } else { return "FALSE"; }
        }
        return buildStringResponse(output);
    }



    private void EvaluateSelectWithoutSuchThat()
    {
        // Console.WriteLine("EvaluateSelectWithoutSuchThat");
        IEnumerable<int> allStmts = [];
        if (select.Item2 == "Procedure")
        {
            IEnumerable<String> allProcedures = pkb.GetAllProcedureNames();
            foreach (var procedure in allProcedures)
            {
                output.Add(procedure);
            }
            Console.WriteLine($"invoked GetAllProceduresNames()");
        }
        else if (select.Item2 == "While")
        {
            // TODO ext IEnumerable<int> allStmts = pkb.GetAllWhiles();
            Console.WriteLine($"invoked GetAllWhiles()");
        }
        else if (select.Item2 == "If")
        {
            // TODO ext IEnumerable<int> allStmts = pkb.GetAllIfs();
            Console.WriteLine($"invoked GetAllIfs()");
        }
        else if (select.Item2 == "Assign")
        {
            // TODO ext IEnumerable<int> allStmts = pkb.GetAssigns();
            Console.WriteLine($"invoked GetAllAssigns()");
        }
        else if (select.Item2 == "Call")
        {
            // TODO ext IEnumerable<int> allStmts = pkb.GetAllCalls();
            Console.WriteLine($"invoked GetAllCalls()");
        }
        else
        {
            // TODO ext IEnumerable<int> allStmts = pkb.GetAllStatements();
            Console.WriteLine($"invoked GetAllStatements()");
        }
        foreach (var stmt in allStmts)
        {
            output.Add(stmt.ToString());
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
                                string modifiedValName = condition.Item5;
                                if (arg1.NodeType == "Procedure")
                                {
                                    // TODO ext IEnumerable<string> modifyingProcedures = pkb.GetProceduresModifying(modifiedValName);
                                    // foreach ()
                                    Console.WriteLine($"invoked GetProceduresModifying({modifiedValName})");
                                }
                                else
                                {
                                    IEnumerable<int> modifyingStatements = pkb.GetStatementsModifying(modifiedValName);
                                    foreach (var line in modifyingStatements)
                                    {
                                        result.Add(line.ToString());
                                    }
                                    Console.WriteLine($"invoked GetStatementsModyfing({modifiedValName})");
                                }
                                break;
                            case "Follows":
                                int followingStatement = Int16.Parse(condition.Item5);
                                int? followedStatement = GetFollowedBy(arg1.NodeType, arg2.NodeType, followingStatement);
                                if (followedStatement != null)
                                {
                                    result.Add(followedStatement.ToString());
                                }
                                break;
                            case "Follows*":
                                int followingTStatement = Int16.Parse(condition.Item5);
                                IEnumerable<int> followedTStaments = pkb.GetAllStatementsLeadingTo(followingTStatement);
                                foreach (var stmt in followedTStaments)
                                {
                                    result.Add(stmt.ToString());
                                }
                                Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({followingTStatement})");
                                break;
                            case "Uses":
                                if (arg1.NodeType == "Procedure")
                                {
                                    // TODO ext IEnumerable<string> usingProcedures = pkb.GetProceduresUsing(usedValName);
                                    // foreach ()
                                    Console.WriteLine($"invoked GetProceduresUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "Assign")
                                {
                                    // TODO ext IEnumerable<int> usingAssignments = pkb.GetAssignmentsUsing(usedValName);
                                    // foreach ()
                                    Console.WriteLine($"invoked GetAssignmentsUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "If")
                                {
                                    // TODO ext IEnumerable<int> usingIfs = pkb.GetIfsUsing(usedValName)
                                    // foreach ()
                                    Console.WriteLine($"invoked pkb.GetIfsUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "While")
                                {
                                    // TODO ext IEnumerable<int> usingWhiles = pkb.GetWhilesUsing(usedValName);
                                    // foreach ()
                                    Console.WriteLine($"invoked GetAssignmentsUsing({condition.Item5})");
                                }
                                else
                                {
                                    string usedValName = condition.Item5;
                                    IEnumerable<int> usingStatements = pkb.GetStatementsUsing(usedValName);
                                    foreach (var stmt in usingStatements)
                                    {
                                        result.Add(stmt.ToString());
                                    }
                                    Console.WriteLine($"invoked GetStatementsUsing({usedValName})");
                                }
                                break;
                            case "Parent":
                                int childStmt = Int16.Parse(condition.Item5);
                                int? parentStmt = pkb.GetParent(childStmt);
                                if (parentStmt != null) result.Add(parentStmt.ToString());
                                Console.WriteLine($"invoked pkb.GetParent({childStmt})");
                                break;
                            case "Parent*":
                                int childTStmt = Int16.Parse(condition.Item5);
                                IEnumerable<int> parentStmts = pkb.GetAllParentStatements(childTStmt);
                                foreach (var stmt in parentStmts)
                                {
                                    result.Add(stmt.ToString());
                                }
                                Console.WriteLine($"invoked pkb.GetAllParentStatements({childTStmt})");
                                break;
                            case "Calls":
                                String callee = condition.Item5;
                                IEnumerable<String> procedures = pkb.GetCallingProcedures(callee);
                                foreach (var procedure in procedures)
                                {
                                    result.Add(procedure);
                                }
                                Console.WriteLine($"invoked pkb.GetCallingProcedures({callee})");
                                break;
                            case "Calls*":
                                //TODO ext IEnumerable<String> procedures = pkb.GetCallingProceduresT(callee);
                                // foreach ()
                                Console.WriteLine($"invoked pkb.GetCallingProceduresT({condition.Item5})");
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
                        if (arg1.NodeType == "Procedure")
                        {
                            // TODO ext IEnumerable<string> allModyfyingProcedures = pkb.GetAllProceduresModifyingAnything();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllProceduresModifyingAnything()");
                        }
                        else
                        {
                            IEnumerable<int> allModyfyingStmts = pkb.GetAllStatementsModifyingAnything();
                            foreach (var stmt in allModyfyingStmts)
                            {
                                result.Add(stmt.ToString());
                            }
                            Console.WriteLine("invoked pkb.GetAllStatementsModyfingAnything()");
                        }
                        break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followingStatements = null;
                        if (arg1.NodeType == "While")
                        {
                            // TODO ext followingStatements = pkb.GetAllFollowedWhiles();
                        }
                        else if (arg1.NodeType == "If")
                        {

                        }
                        else if (arg1.NodeType == "Assign")
                        {

                        }
                        else if (arg1.NodeType == "Call")
                        {

                        }
                        else
                        {
                            followingStatements = pkb.GetAllFollowsTargets();
                            foreach (var stmt in followingStatements)
                            {
                                result.Add(stmt.ToString());
                            }
                            Console.WriteLine("invoked pkb.GetAllFollowsSources()");
                        }
                        break;
                    case "Uses":
                        if (arg1.NodeType == "Procedure")
                        {
                            // TODO ext IEnumerable<String> proceduresUsingAnything = pkb.GetAllProceduresUsingAnything();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllProceduresUsingAnything()");
                        }
                        else if (arg1.NodeType == "Assign")
                        {
                            // TODO ext IEnumerable<int> assignmentsUsingAnything = pkb.GetAllAssignmentsUsingAnything();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllAssignmentsUsingAnything()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            // TODO ext IEnumerable<int> whilesUsingAnything = pkb.GetAllWhilesUsingAnything();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllWhilesUsingAnything()");
                        }
                        else if (arg1.NodeType == "If")
                        {
                            // TODO ext IEnumerable<int> ifsUsingAnything = pkb.GetAllIfsUsingAnything();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllIfsUsingAnything()");
                        }
                        else
                        {
                            IEnumerable<int> stmtsUsingAnything = pkb.GetAllStatementsUsingAnything();
                            foreach (var stmt in stmtsUsingAnything)
                            {
                                result.Add(stmt.ToString());
                            }
                            Console.WriteLine("invoked pkb.GetAllStatementsUsingAnything()");
                        }
                        break;
                    case "Parent":
                    case "Parent*":
                        if (arg1.NodeType == "If")
                        {
                            // TODO ext IEnumerable<int> allParentIfs = pkb.GetAllParentIfs();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllParentIfs()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            // TODO ext IEnumerable<int> allParentWhiles = pkb.GetAllParentWhiles();
                            // foreach ()
                            Console.WriteLine("invoked pkb.GetAllParentWhiles()");
                        }
                        else
                        {
                            IEnumerable<int> allParents = pkb.GetAllParentStatements();
                            foreach (var parent in allParents)
                            {
                                result.Add(parent.ToString());
                            }
                            Console.WriteLine("invoked pkb.GetAllParentStatements()");
                        }
                        break;
                    case "Calls":
                    case "Calls*":
                        //TODO ext IEnumerable<string> allCallers = pkb.GetAllCallingProcedures();
                        // foreach ()
                        Console.WriteLine("invoked pkb.GetAllCallingProcedures()");
                        break;
                }
            }
        }
        else if (select.Item1 == arg2.Name)
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
                            if (arg1.NodeType == "Procedure")
                            {
                                // TODO ext IEnumerable<String> modifiedVariables = pkb.GetModifiedVariables(modyfyingProcName);
                                // foreach ()
                                Console.WriteLine($"invoked GetModifiedVariables({condition.Item5})");
                            }
                            else
                            {
                                int modyfyingStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> modifiedVariables = pkb.GetModifiedVariables(modyfyingStmtNumber);
                                foreach (var variable in modifiedVariables)
                                {
                                    result.Add(variable);
                                }
                                Console.WriteLine($"invoked GetModifiedVariables({modyfyingStmtNumber})");
                            }

                            break;
                        case "Follows":
                            int followedStmt = Int16.Parse(condition.Item5);
                            int? followingStatement = pkb.GetFollows(followedStmt);
                            if (followingStatement != null) result.Add(followingStatement.ToString());
                            Console.WriteLine($"invoked pkb.GetFollows({followedStmt})");
                            break;
                        case "Follows*":
                            int followedTStmt = Int16.Parse(condition.Item5);
                            IEnumerable<int> followingStatements = pkb.GetAllFollowingStatements(followedTStmt);
                            foreach (var stmt in followingStatements)
                            {
                                result.Add(stmt.ToString());
                            }
                            Console.WriteLine($"invoked pkb.GetAllFollowingStatements({followedTStmt})");
                            break;
                        case "Uses":
                            if (arg1.NodeType == "Procedure")
                            {
                                // TODO ext IEnumerable<String> usedVariables = pkb.GetUsedVariables(procName);
                                // foreach ()
                                Console.WriteLine($"invoked GetUsedVariables({condition.Item5})");
                            }
                            else if (arg1.NodeType == "Assign")
                            {
                                int usingAssignmentsStmtNumber = Int16.Parse(condition.Item5);
                                // TODO ext IEnumerable<String> usedVariables = pkb.GetUsedVariablesByAssign(int usingAssignStmtNumber)
                                // foreach ()
                                Console.WriteLine($"invoked pkb.GetUsedVariablesByAssign({usingAssignmentsStmtNumber})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                int usingWhileStmtNumber = Int16.Parse(condition.Item5);
                                // TODO ext IEnumerable<String> usedVariables = pkb.GetUsedVariablesByWhile(usingWhileStmtNumber)
                                // foreach ()
                                Console.WriteLine($"invoked pkb.GetUsedVariablesByWhile({usingWhileStmtNumber})");
                            }
                            else if (arg1.NodeType == "If")
                            {
                                int usingIfStmtNumber = Int16.Parse(condition.Item5);
                                // TODO ext IEnumerable<String> usedVariables = pkb.GetUsedVariablesByIf(usingIfStmtNumber)
                                // foreach ()
                                Console.WriteLine($"invoked pkb.GetUsedVariablesByIf({usingIfStmtNumber})");
                            }
                            else
                            {
                                int usingStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> usedVariables = pkb.GetUsedVariables(usingStmtNumber);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                Console.WriteLine($"invoked GetUsedVariables({usingStmtNumber})");
                            }

                            break;
                        case "Parent":
                            int parentStmt = Int16.Parse(condition.Item5);
                            IEnumerable<int> childrenStmts = [];
                            if (arg1.NodeType == "If")
                            {
                                // TODO ext IEnumerable<int> childrenStmts = pkb.GetChildrenOfIf(int ifStmt);
                                Console.WriteLine($"invoked pkb.GetChildrenOfIf({parentStmt})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                // TODO ext IEnumerable<int> childrenStmts = pkb.GetChildrenOfWhile(int whileStmt);
                                Console.WriteLine($"invoked pkb.GetChildrenOfWhile({parentStmt})");
                            }
                            else
                            {
                                childrenStmts = pkb.GetChildren(parentStmt);
                                Console.WriteLine($"invoked pkb.GetChildren({parentStmt})");
                            }
                            foreach (var stmt in childrenStmts)
                            {
                                result.Add(stmt.ToString());
                            }
                            break;
                        case "Parent*":
                            int parentTStmt = Int16.Parse(condition.Item5);
                            IEnumerable<int> childStmts = [];
                            if (arg1.NodeType == "If")
                            {
                                // TODO ext IEnumerable<int> childStmts = pkb.GetAllNestedStatementsInIfT(int whileTStmt);
                                Console.WriteLine($"invoked pkb.GetAllNestedStatementsInIfT({parentTStmt})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                // TODO ext IEnumerable<int> childStmts = pkb.GetAllNestedStatementsInWhileT(int whileTStmt);
                                Console.WriteLine($"invoked pkb.GetAllNestedStatementsInWhileT({parentTStmt})");
                            }
                            else
                            {
                                childStmts = pkb.GetAllNestedStatements(parentTStmt);
                                Console.WriteLine($"invoked pkb.GetAllNestedStatements({parentTStmt})");
                            }
                            foreach (var stmt in childStmts)
                            {
                                result.Add(stmt.ToString());
                            }
                            break;
                        case "Calls":
                            String caller = condition.Item5;
                            IEnumerable<String> procedures = pkb.GetCalledProcedures(caller);
                            foreach (var procedure in procedures)
                            {
                                result.Add(procedure);
                            }
                            Console.WriteLine($"invoked pkb.GetCalledProcedures({caller})");
                            break;
                        case "Calls*":
                            // TODO ext IEnumerable<String> procedures = pkb.GetCalledProceduresT(caller);
                            // foreach ()
                            Console.WriteLine($"invoked pkb.GetCalledProceduresT({condition.Item5})");
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
                        Console.WriteLine("invoked pkb.GetAllModifiedVariables()");
                        break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followedStatements = pkb.GetAllFollowsSources();
                        foreach (var stmt in followedStatements)
                        {
                            result.Add(stmt.ToString());
                        }
                        Console.WriteLine("invoked pkb.GetAllFollowsTargets()");
                        break;
                    case "Uses":
                        IEnumerable<string> allUsedVariables = pkb.GetAllUsedVariables();
                        foreach (var variable in allUsedVariables)
                        {
                            result.Add(variable);
                        }
                        Console.WriteLine("invoked GetAllUsedVariables()");
                        break;
                    case "Parent":
                        IEnumerable<int> allChildren = [];
                        if (arg1.NodeType == "If")
                        {
                            // TODO ext IEnumerable<int> allChildren = pkb.GetAllChildStatementsOfIfs();
                            Console.WriteLine("invoked pkb.GetAllChildStatementsOfIfs()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            // TODO ext IEnumerable<int> allChildren = pkb.GetAllChildStatementsOfWhiles();
                            Console.WriteLine("invoked pkb.GetAllChildStatementsOfWhiles()");
                        }
                        else
                        {
                            allChildren = pkb.GetAllChildStatements();
                            Console.WriteLine("invoked pkb.GetAllChildStatements()");
                        }
                        foreach (var child in allChildren)
                        {
                            result.Add(child.ToString());
                        }
                        break;
                    case "Parent*":
                        IEnumerable<int> allChildrenT = [];
                        if (arg1.NodeType == "If")
                        {
                            // TODO ext IEnumerable<int> allChildren = pkb.GetAllChildStatementsOfIfsT();
                            Console.WriteLine("invoked pkb.GetAllChildStatementsOfIfsT()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            // TODO ext IEnumerable<int> allChildren = pkb.GetAllChildStatementsOfWhilesT();
                            Console.WriteLine("invoked pkb.GetAllChildStatementsOfWhilesT()");
                        }
                        else
                        {
                            allChildrenT = pkb.GetAllChildStatements();
                            Console.WriteLine("invoked pkb.GetAllChildStatements()");
                        }
                        foreach (var child in allChildrenT)
                        {
                            result.Add(child.ToString());
                        }
                        break;
                    case "Calls":
                    case "Calls*":
                        //TODO ext IEnumerable<String> procedures = pkb.GetAllCalledProcedures();
                        break;
                }
            }
        }
        else
        {
            //nie tak to powinno wyglądać ale już tego nie poprawię
            EvaluateRelationWithParameter<int>(relation, true, 1);
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
                        int? follower = pkb.GetFollows(intParameter);
                        if (follower != null) result.Add(follower.ToString());
                        Console.WriteLine($"invoked pkb.GetFollows({intParameter})");   
                    }
                    else
                    {
                        int? followed = pkb.GetFollowedBy(intParameter);
                        if (followed != null) result.Add(followed.ToString());
                        Console.WriteLine($"invoked pkb.GetFollowedBy({intParameter})"); 
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
                        Console.WriteLine($"invoked pkb.GetAllFollowingStatements({intParameter})"); 
                    } 
                    else
                    {
                        IEnumerable<int> precedents = pkb.GetAllStatementsLeadingTo(intParameter);
                        foreach (var precedent in precedents)
                        {
                            result.Add(precedent.ToString());
                        }
                        Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({intParameter})");   
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
                        Console.WriteLine($"invoked pkb.GetUsedVariables({intParameter})"); 
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
                        Console.WriteLine($"invoked pkb.GetModifiedVariables({intParameter})"); 
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
                        Console.WriteLine($"invoked pkb.GetChildren({intParameter})"); 
                    } 
                    else
                    {
                        int? parent = pkb.GetParent(intParameter);
                        if (parent != null) result.Add(parent.ToString());
                        Console.WriteLine($"invoked pkb.GetParent({intParameter})");   
                    }      
                break;
                case "Parent*":
                    IEnumerable<int> parentStmts = pkb.GetAllParentStatements(intParameter);
                    foreach (var stmt in parentStmts)
                    {
                        result.Add(stmt.ToString());
                    }
                    Console.WriteLine($"invoked pkb.GetAllParentStatements({intParameter})");  
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
                        // TODO ext IEnumerable<string> variables = pkb.GetModifiedVariables(procName);
                        // foreach (var variable in variables)
                        // {
                        //     result.Add(variable.ToString());
                        // }
                        // Console.WriteLine($"invoked pkb.GetModifiedVariables({stringParameter})"); 
                    }
                    else 
                    {
                        IEnumerable<string> procedures = pkb.GetProceduresModifying(stringParameter);
                        foreach (var procedure in procedures)
                        {
                            result.Add(procedure.ToString());
                        }
                        Console.WriteLine($"invoked pkb.GetProceduresModifying({stringParameter})");
                    }     
                break;
                case "Uses":
                    if (isFirstArgParameter)
                    {
                        // TODO ext IEnumerable<string> procedures = pkb.GetProceduresUsing(varName)
                        // foreach()
                        Console.WriteLine($"invoked pkb.pkb.GetProceduresUsing({stringParameter})"); 
                    }
                    else
                    {
                        // TODO ext IEnumerable<string> variables = pkb.GetUsedVariablesByProcedure(procName);
                        // foreach (var variable in variables)
                        // {
                        //     result.Add(variable.ToString());
                        // }
                        Console.WriteLine($"invoked pkb.GetUsedVariablesByProcedure({stringParameter})"); 
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
                        Console.WriteLine($"invoked pkb.GetCallingProcedures({stringParameter})"); 
                    }
                    else
                    {
                        IEnumerable<String> procedures = pkb.GetCalledProcedures(stringParameter);
                        foreach (var procedure in procedures)
                        {
                            result.Add(procedure.ToString());
                        }
                        Console.WriteLine($"invoked pkb.GetCalledProcedures({stringParameter})"); 
                    }
                break;
                case "Calls*":
                    if (isFirstArgParameter)
                    {
                        // TODO ext IEnumerable<String> procedures = pkb.GetCallingProceduresT(called);
                    }
                    else 
                    {
                        // TODO ext IEnumerable<String> procedures = pkb.GetCalledProceduresT(calling);
                    }


                break;
            }
        }
        return result;
    }

    int? GetFollowedBy(String followed, String following, int followingStmtNumber)
    {
        int? result = null;
        switch (followed)
        {
            case "If": // Jeszcze do dodania switch (following) ale poczekam na info z pkb
                // TODO ext result = GetFollowedByIf(followingStmtNumber);
                Console.WriteLine($"invoked pkb.GetFollowedByIf({followingStmtNumber})");
                break;
            case "While":
                // TODO ext result = pkb.GetFollowedByWhile(followingStmtNumber)
                Console.WriteLine($"invoked pkb.GetFollowedByWhile({followingStmtNumber})");
                break;
            case "Call":
                // TODO ext result = pkb.GetFollowedByCall(followingStmtNumber);
                Console.WriteLine($"invoked pkb.GetFollowedByCall({followingStmtNumber})");
                break;
            case "Assign":
                // TODO ext result = pkb.GetFollowedByAssign("Assign", followingStmtNumber);
                Console.WriteLine($"invoked pkb.GetFollowedByAssign({followingStmtNumber})");
                break;
            default:
                result = pkb.GetFollowedBy(followingStmtNumber);
                Console.WriteLine($"invoked pkb.GetFollowedBy({followingStmtNumber})");
                break;
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
