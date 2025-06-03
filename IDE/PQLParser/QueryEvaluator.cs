using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.SIMPLE;
using Atsi.Structures.SIMPLE.Statements;
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
                foreach (var w in with)
                {
                    //Console.WriteLine("with: " + w.ToString());
                }
            }
        }
        foreach (var child in tree.Children)
        {
            if (child.NodeType == "suchthat")
            {
                HashSet<String> partialOutput = [];
                isSuchThatPresent = true;
                foreach (var relation in child.Children)
                {
                    var arg1 = relation.Children[0];
                    var arg2 = relation.Children[1];
                    partialOutput = EvaluateRelation(relation.Name, arg1, arg2);
                    if (partialOutput.Count == 0)
                    {
                        if (select.Item1 == "Boolean")
                        {
                            return "false";
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
            if (output.Count > 0) { return "true"; } else { return "false"; }
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
            //Console.WriteLine($"invoked GetAllProceduresNames()");
        }
        else if (select.Item2 == "While")
        {
            allStmts = pkb.GetAllWhiles();
            //Console.WriteLine($"invoked GetAllWhiles()");
        }
        else if (select.Item2 == "If")
        {
            allStmts = pkb.GetAllIfs();
            //Console.WriteLine($"invoked GetAllIfs()");
        }
        else if (select.Item2 == "Assign")
        {
            allStmts = pkb.GetAllAssigns();
            //Console.WriteLine($"invoked GetAllAssigns()");
        }
        else if (select.Item2 == "Call")
        {
            allStmts = pkb.GetAllCallsNumbers();
            //Console.WriteLine($"invoked GetAllCalls()");
        }
        else
        {
            allStmts = pkb.GetAllStatements();
            //Console.WriteLine($"invoked GetAllStatements()");
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
                result = EvaluateRelationWithParameter<int>(relation, false, Int32.Parse(arg2.Name), arg1.NodeType, arg2.NodeType);
            }
            if (arg2.NodeType == "String")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<String>(relation, false, arg2.Name, arg1.NodeType, arg2.NodeType);
            }
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
                                    IEnumerable<string> modifyingProcedures = pkb.GetProceduresModifying(modifiedValName);
                                    foreach (var procedure in modifyingProcedures)
                                    {
                                        result.Add(procedure);
                                    }
                                    //Console.WriteLine($"invoked GetProceduresModifying({modifiedValName})");
                                }
                                else
                                {
                                    IEnumerable<int> modifyingStatements = pkb.GetStatementsModifying(modifiedValName);
                                    foreach (var line in modifyingStatements)
                                    {
                                        result.Add(line.ToString());
                                    }
                                    //Console.WriteLine($"invoked GetStatementsModyfing({modifiedValName})");
                                }
                                break;
                            case "Follows":
                                int followingStatement = Int16.Parse(condition.Item5);
                                int? followedStatement = EvaluateFollows(followingStatement, false, arg1.NodeType, arg2.NodeType);
                                if (followedStatement != null)
                                {
                                    result.Add(followedStatement.ToString());
                                }
                                break;
                            case "Follows*":
                                int followingTStatement = Int16.Parse(condition.Item5);
                                IEnumerable<int> followedTStaments = EvaluateFollowsT(followingTStatement, false, arg1.NodeType, arg2.NodeType);
                                foreach (var stmt in followedTStaments)
                                {
                                    result.Add(stmt.ToString());
                                }
                                break;
                            case "Uses":
                                if (arg1.NodeType == "Procedure")
                                {
                                    IEnumerable<string> usingProcedures = pkb.GetProceduresUsing(condition.Item5);
                                    foreach (var procedure in usingProcedures)
                                    {
                                        result.Add(procedure);
                                    }
                                    //Console.WriteLine($"invoked GetProceduresUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "Assign")
                                {
                                    IEnumerable<int> usingAssignments = pkb.GetAssignmentsUsing(condition.Item5);
                                    foreach (var assignment in usingAssignments)
                                    {
                                        result.Add(assignment.ToString());
                                    }
                                    //Console.WriteLine($"invoked GetAssignmentsUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "If")
                                {
                                    IEnumerable<int> usingIfs = pkb.GetIfsUsing(condition.Item5);
                                    foreach (var i in usingIfs)
                                    {
                                        result.Add(i.ToString());
                                    }
                                    //Console.WriteLine($"invoked pkb.GetIfsUsing({condition.Item5})");
                                }
                                else if (arg1.NodeType == "While")
                                {
                                    IEnumerable<int> usingWhiles = pkb.GetWhilesUsing(condition.Item5);
                                    foreach (var whilee in usingWhiles)
                                    {
                                        result.Add(whilee.ToString());
                                    }
                                    //Console.WriteLine($"invoked GetAssignmentsUsing({condition.Item5})");
                                }
                                else
                                {
                                    string usedValName = condition.Item5;
                                    IEnumerable<int> usingStatements = pkb.GetStatementsUsing(usedValName);
                                    foreach (var stmt in usingStatements)
                                    {
                                        result.Add(stmt.ToString());
                                    }
                                    //Console.WriteLine($"invoked GetStatementsUsing({usedValName})");
                                }
                                break;
                            case "Parent":
                                int childStmt = Int16.Parse(condition.Item5);
                                int? parentStmt = pkb.GetParent(childStmt);
                                if (parentStmt != null) result.Add(parentStmt.ToString());
                                //Console.WriteLine($"invoked pkb.GetParent({childStmt})");
                                break;
                            case "Parent*":
                                int childTStmt = Int16.Parse(condition.Item5);
                                IEnumerable<int> parentStmts = pkb.GetAllParentStatements(childTStmt);
                                foreach (var stmt in parentStmts)
                                {
                                    result.Add(stmt.ToString());
                                }
                                //Console.WriteLine($"invoked pkb.GetAllParentStatements({childTStmt})");
                                break;
                            case "Calls":
                                String callee = condition.Item5;
                                IEnumerable<String> procedures = pkb.GetCallingProcedures(callee);
                                foreach (var procedure in procedures)
                                {
                                    result.Add(procedure);
                                }
                                //Console.WriteLine($"invoked pkb.GetCallingProcedures({callee})");
                                break;
                            case "Calls*":
                                IEnumerable<String> procs = pkb.GetCallingProceduresT(condition.Item5);
                                foreach (var procedure in procs)
                                {
                                    result.Add(procedure);
                                }
                                //Console.WriteLine($"invoked pkb.GetCallingProceduresT({condition.Item5})");
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
                            IEnumerable<string> allModyfyingProcedures = pkb.GetAllProceduresModifyingAnything();
                            foreach (var procedure in allModyfyingProcedures)
                            {
                                result.Add(procedure);
                            }
                            //Console.WriteLine("invoked pkb.GetAllProceduresModifyingAnything()");
                        }
                        else
                        {
                            IEnumerable<int> allModyfyingStmts = pkb.GetAllStatementsModifyingAnything();
                            foreach (var stmt in allModyfyingStmts)
                            {
                                result.Add(stmt.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllStatementsModyfingAnything()");
                        }
                        break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followedStatements = null;
                        if (arg1.NodeType != "stmt")
                        {
                            followedStatements = pkb.GetFollowedStatementsByType(arg1.NodeType);
                            //Console.WriteLine($"invoked pkb.GetFollowedStatementsByType({arg1.NodeType})");
                        }
                        else
                        {
                            followedStatements = pkb.GetAllFollowsSources();
                            //Console.WriteLine("invoked pkb.GetAllFollowsSources()");
                        }
                        foreach (var stmt in followedStatements)
                        {
                            result.Add(stmt.ToString());
                        }
                        break;
                    case "Uses":
                        if (arg1.NodeType == "Procedure")
                        {
                            IEnumerable<String> proceduresUsingAnything = pkb.GetAllProceduresUsingAnything();
                            foreach (var procedure in proceduresUsingAnything)
                            {
                                result.Add(procedure.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllProceduresUsingAnything()");
                        }
                        else if (arg1.NodeType == "Assign")
                        {
                            IEnumerable<int> assignmentsUsingAnything = pkb.GetAllAssignmentsUsingAnything();
                            foreach (var assignment in assignmentsUsingAnything)
                            {
                                result.Add(assignment.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllAssignmentsUsingAnything()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            IEnumerable<int> whilesUsingAnything = pkb.GetAllWhilesUsingAnything();
                            foreach (var whilee in whilesUsingAnything)
                            {
                                result.Add(whilee.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllWhilesUsingAnything()");
                        }
                        else if (arg1.NodeType == "If")
                        {
                            IEnumerable<int> ifsUsingAnything = pkb.GetAllIfsUsingAnything();
                            foreach (var i in ifsUsingAnything)
                            {
                                result.Add(i.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllIfsUsingAnything()");
                        }
                        else
                        {
                            IEnumerable<int> stmtsUsingAnything = pkb.GetAllStatementsUsingAnything();
                            foreach (var stmt in stmtsUsingAnything)
                            {
                                result.Add(stmt.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllStatementsUsingAnything()");
                        }
                        break;
                    case "Parent":
                    case "Parent*":
                        if (arg1.NodeType == "If")
                        {
                            IEnumerable<int> allParentIfs = pkb.GetAllParentIfs();
                            foreach (var i in allParentIfs)
                            {
                                result.Add(i.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllParentIfs()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            IEnumerable<int> allParentWhiles = pkb.GetAllParentWhiles();
                            foreach (var whilee in allParentWhiles)
                            {
                                result.Add(whilee.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllParentWhiles()");
                        }
                        else
                        {
                            IEnumerable<int> allParents = pkb.GetAllParentStatements();
                            foreach (var parent in allParents)
                            {
                                result.Add(parent.ToString());
                            }
                            //Console.WriteLine("invoked pkb.GetAllParentStatements()");
                        }
                        break;
                    case "Calls":
                    case "Calls*":
                        IEnumerable<string> allCallers = pkb.GetAllCallingProcedures();
                        foreach (var caller in allCallers)
                        {
                            result.Add(caller.ToString());
                        }
                        // Console.WriteLine("invoked pkb.GetAllCallingProcedures()");
                        break;
                }
            }
        }
        else if (select.Item1 == arg2.Name)
        {
            if (arg1.NodeType == "Number")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<int>(relation, true, Int32.Parse(arg1.Name), arg1.NodeType, arg2.NodeType);
            }
            if (arg1.NodeType == "String")
            {
                isNotConditioned = false;
                result = EvaluateRelationWithParameter<String>(relation, true, arg1.Name, arg1.NodeType, arg2.NodeType);
            }
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
                                IEnumerable<String> modifiedVariables = pkb.GetModifiedVariables(condition.Item5);
                                foreach (var variable in modifiedVariables)
                                {
                                    result.Add(variable.ToString());
                                }
                                //Console.WriteLine($"invoked GetModifiedVariables({condition.Item5})");
                            }
                            else
                            {
                                int modyfyingStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> modifiedVariables = pkb.GetModifiedVariables(modyfyingStmtNumber);
                                foreach (var variable in modifiedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked GetModifiedVariables({modyfyingStmtNumber})");
                            }

                            break;
                        case "Follows":
                            int followedStmt = Int16.Parse(condition.Item5);
                            int? followingStatement = pkb.GetFollows(followedStmt);
                            if (followingStatement != null) result.Add(followingStatement.ToString());
                            //Console.WriteLine($"invoked pkb.GetFollows({followedStmt})");
                            break;
                        case "Follows*":
                            int followedTStmt = Int16.Parse(condition.Item5);
                            IEnumerable<int> followingStatements = pkb.GetAllFollowingStatements(followedTStmt);
                            foreach (var stmt in followingStatements)
                            {
                                result.Add(stmt.ToString());
                            }
                            //Console.WriteLine($"invoked pkb.GetAllFollowingStatements({followedTStmt})");
                            break;
                        case "Uses":
                            if (arg1.NodeType == "Procedure")
                            {
                                IEnumerable<String> usedVariables = pkb.GetUsedVariables(condition.Item5);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked GetUsedVariables({condition.Item5})");
                            }
                            else if (arg1.NodeType == "Assign")
                            {
                                int usingAssignmentsStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> usedVariables = pkb.GetUsedVariablesByAssign(usingAssignmentsStmtNumber);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked pkb.GetUsedVariablesByAssign({usingAssignmentsStmtNumber})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                int usingWhileStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> usedVariables = pkb.GetUsedVariablesByWhile(usingWhileStmtNumber);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked pkb.GetUsedVariablesByWhile({usingWhileStmtNumber})");
                            }
                            else if (arg1.NodeType == "If")
                            {
                                int usingIfStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> usedVariables = pkb.GetUsedVariablesByIf(usingIfStmtNumber);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked pkb.GetUsedVariablesByIf({usingIfStmtNumber})");
                            }
                            else
                            {
                                int usingStmtNumber = Int16.Parse(condition.Item5);
                                IEnumerable<String> usedVariables = pkb.GetUsedVariables(usingStmtNumber);
                                foreach (var variable in usedVariables)
                                {
                                    result.Add(variable);
                                }
                                //Console.WriteLine($"invoked GetUsedVariables({usingStmtNumber})");
                            }

                            break;
                        case "Parent":
                            int parentStmt = Int16.Parse(condition.Item5);
                            IEnumerable<int> childrenStmts = [];
                            if (arg1.NodeType == "If")
                            {
                                childrenStmts = pkb.GetChildrenOfIf(parentStmt);
                                //Console.WriteLine($"invoked pkb.GetChildrenOfIf({parentStmt})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                childrenStmts = pkb.GetChildrenOfWhile(parentStmt);
                                //Console.WriteLine($"invoked pkb.GetChildrenOfWhile({parentStmt})");
                            }
                            else
                            {
                                childrenStmts = pkb.GetChildren(parentStmt);
                                //Console.WriteLine($"invoked pkb.GetChildren({parentStmt})");
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
                                childStmts = pkb.GetAllNestedStatementsInIfT(parentTStmt);
                                //Console.WriteLine($"invoked pkb.GetAllNestedStatementsInIfT({parentTStmt})");
                            }
                            else if (arg1.NodeType == "While")
                            {
                                childStmts = pkb.GetAllNestedStatementsInWhileT(parentTStmt);
                                //Console.WriteLine($"invoked pkb.GetAllNestedStatementsInWhileT({parentTStmt})");
                            }
                            else
                            {
                                childStmts = pkb.GetAllNestedStatements(parentTStmt);
                                //Console.WriteLine($"invoked pkb.GetAllNestedStatements({parentTStmt})");
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
                            //Console.WriteLine($"invoked pkb.GetCalledProcedures({caller})");
                            break;
                        case "Calls*":
                            procedures = pkb.GetCalledProceduresT(condition.Item5);
                            foreach (var procedure in procedures)
                            {
                                result.Add(procedure);
                            }
                            //Console.WriteLine($"invoked pkb.GetCalledProceduresT({condition.Item5})");
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
                        //Console.WriteLine("invoked pkb.GetAllModifiedVariables()");
                        break;
                    case "Follows":
                    case "Follows*":
                        IEnumerable<int> followingStatements = pkb.GetAllFollowsTargets();
                        foreach (var stmt in followingStatements)
                        {
                            result.Add(stmt.ToString());
                        }
                        //Console.WriteLine("invoked pkb.GetAllFollowsTargets()");
                        break;
                    case "Uses":
                        IEnumerable<string> allUsedVariables = pkb.GetAllUsedVariables();
                        foreach (var variable in allUsedVariables)
                        {
                            result.Add(variable);
                        }
                        //Console.WriteLine("invoked GetAllUsedVariables()");
                        break;
                    case "Parent":
                        IEnumerable<int> allChildren = [];
                        if (arg1.NodeType == "If")
                        {
                            allChildren = pkb.GetAllChildStatementsOfIfs();
                            //Console.WriteLine("invoked pkb.GetAllChildStatementsOfIfs()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            allChildren = pkb.GetAllChildStatementsOfWhiles();
                            //Console.WriteLine("invoked pkb.GetAllChildStatementsOfWhiles()");
                        }
                        else
                        {
                            allChildren = pkb.GetAllChildStatements();
                            //Console.WriteLine("invoked pkb.GetAllChildStatements()");
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
                            allChildren = pkb.GetAllChildStatementsOfIfsT();
                            //Console.WriteLine("invoked pkb.GetAllChildStatementsOfIfsT()");
                        }
                        else if (arg1.NodeType == "While")
                        {
                            allChildren = pkb.GetAllChildStatementsOfWhilesT();
                            //Console.WriteLine("invoked pkb.GetAllChildStatementsOfWhilesT()");
                        }
                        else
                        {
                            allChildrenT = pkb.GetAllChildStatements();
                            //Console.WriteLine("invoked pkb.GetAllChildStatements()");
                        }
                        foreach (var child in allChildrenT)
                        {
                            result.Add(child.ToString());
                        }
                        break;
                    case "Calls":
                    case "Calls*":
                        IEnumerable<String> procedures = pkb.GetAllCalledProcedures();
                        break;
                }
            }
        }
        else
        {
            //nie tak to powinno wyglądać ale już tego nie poprawię
            EvaluateRelationWithParameter<int>(relation, true, 1, arg1.NodeType, arg2.NodeType);
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

    private HashSet<String> EvaluateRelationWithParameter<T>(String relation, bool isFirstArgParameter, T parameter, string arg1Type, string arg2Type)
    {
        HashSet<String> result = [];
        if (parameter is int intParameter)
        {
            switch(relation)
            {
                case "Follows":
                    if (isFirstArgParameter)
                    {

                        int? follower = EvaluateFollows(intParameter, isFirstArgParameter, following: arg2Type);
                        if (follower != null) result.Add(follower.ToString());
                    }
                    else
                    {
                        int? followed = EvaluateFollows(intParameter, isFirstArgParameter, followed: arg1Type);
                        if (followed != null) result.Add(followed.ToString());
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
                        //Console.WriteLine($"invoked pkb.GetAllFollowingStatements({intParameter})"); 
                    } 
                    else
                    {
                        IEnumerable<int> precedents = pkb.GetAllStatementsLeadingTo(intParameter);
                        foreach (var precedent in precedents)
                        {
                            result.Add(precedent.ToString());
                        }
                        //Console.WriteLine($"invoked pkb.GetAllStatementsLeadingTo({intParameter})");   
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
                        //Console.WriteLine($"invoked pkb.GetUsedVariables({intParameter})"); 
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
                        //Console.WriteLine($"invoked pkb.GetModifiedVariables({intParameter})"); 
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
                        //Console.WriteLine($"invoked pkb.GetChildren({intParameter})"); 
                    } 
                    else
                    {
                        int? parent = pkb.GetParent(intParameter);
                        if (parent != null) result.Add(parent.ToString());
                        //Console.WriteLine($"invoked pkb.GetParent({intParameter})");   
                    }      
                break;
                case "Parent*":
                    IEnumerable<int> parentStmts = pkb.GetAllParentStatements(intParameter);
                    foreach (var stmt in parentStmts)
                    {
                        result.Add(stmt.ToString());
                    }
                    //Console.WriteLine($"invoked pkb.GetAllParentStatements({intParameter})");  
                break;
            }
        }
        if (parameter is String stringParameter)
        {
            IEnumerable<String> procedures = [];
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
                        //Console.WriteLine($"invoked pkb.GetModifiedVariables({stringParameter})");
                    }
                    else
                    {
                        procedures = pkb.GetProceduresModifying(stringParameter);
                        //Console.WriteLine($"invoked pkb.GetProceduresModifying({stringParameter})");
                    }
                    break;
                case "Uses":
                    if (isFirstArgParameter)
                    {
                        procedures = pkb.GetProceduresUsing(stringParameter);
                        //Console.WriteLine($"invoked pkb.pkb.GetProceduresUsing({stringParameter})");
                    }
                    else
                    {
                        IEnumerable<string> variables = pkb.GetUsedVariablesByProcedure(stringParameter);
                        foreach (var variable in variables)
                        {
                            result.Add(variable.ToString());
                        }
                        //Console.WriteLine($"invoked pkb.GetUsedVariablesByProcedure({stringParameter})");
                    }
                    break;
                case "Calls":
                    if (isFirstArgParameter)
                    {
                        
                        procedures = pkb.GetCalledProcedures(stringParameter);
                        //Console.WriteLine($"invoked pkb.GetCalledProcedures({stringParameter})");
                    }
                    else
                    {
                        procedures = pkb.GetCallingProcedures(stringParameter);
                        //Console.WriteLine($"invoked pkb.GetCallingProcedures({stringParameter})");
                    }
                    break;
                case "Calls*":
                    if (isFirstArgParameter)
                    {
                        procedures = pkb.GetCallingProceduresT(stringParameter);
                    }
                    else
                    {
                        procedures = pkb.GetCalledProceduresT(stringParameter);
                    }
                    break;
            }
            foreach (var procedure in procedures)
            {
                result.Add(procedure.ToString());
            }
        }
        return result;
    }

    int? EvaluateFollows(int intParameter, bool isFirstArgParameter, String followed = "", String following ="", bool isTransitive = false)
    {
        int? result = null;
        switch (followed)
        {
            case "":
                break;
            case "If":
                    result = pkb.GetFollowedByIf(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByIf({intParameter})");
                break;
            case "While":
                    result = pkb.GetFollowedByWhile(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByWhile({intParameter})");
                break;
            case "Call":
                    result = pkb.GetFollowedByCall(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByCall({intParameter})");
                break;
            case "Assign":
                    result = pkb.GetFollowedByAssign(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByAssign({intParameter})");
                break;
            default:
                result = pkb.GetFollowedBy(intParameter);
                //Console.WriteLine($"invoked pkb.GetFollowedBy({intParameter})");
                break;
        }
        switch (following)
        {
            case "":
                break;
            case "If":
                    result = pkb.GetIfFollowing(intParameter);
                    //Console.WriteLine($"invoked pkb.GetIfFollowing({intParameter})");  
                break;
            case "While":
                    result = pkb.GetWhileFollowing(intParameter);
                    //Console.WriteLine($"invoked pkb.GetWhileFollowing({intParameter})");
                break;
            case "Call":
                    result = pkb.GetCallFollowing(intParameter);
                    //Console.WriteLine($"invoked pkb.GetCallFollowing({intParameter})");
                break;
            case "Assign":
                    result = pkb.GetAssignFollowing(intParameter);
                    //Console.WriteLine($"invoked pkb.GetAssignFollowing({intParameter})");
                break;
            default:
                result = pkb.GetFollowedBy(intParameter);
                //Console.WriteLine($"invoked pkb.GetFollowedBy({intParameter})");
                break;
        }
        return result;
    }

    IEnumerable<int> EvaluateFollowsT(int intParameter, bool isFirstArgParameter,String followed, String following)
    {
        IEnumerable<int> result = [];

        if (isFirstArgParameter)
        {
            switch (following)
            {
                case "":
                    break;
                case "If":
                    switch (followed)
                    {
                        case "":
                            break;
                        case "If":
                            
                        case "While":
                            
                        case "Call":
                            
                        case "Assign":
                            
                        default:
                            result = pkb.GetIfsFollowingT(intParameter);
                            //Console.WriteLine($"invoked pkb.GetIfsFollowingT({intParameter})");
                            break;
                    }

                    break;
                case "While":
                    result = pkb.GetWhilesFollowingT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetWhilesFollowingT({intParameter})");
                    break;
                case "Call":
                    result = pkb.GetCallsFollowingT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetCallsFollowingT({intParameter})");
                    break;
                case "Assign":
                    result = pkb.GetAssignsFollowingT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetAssignsFollowingT({intParameter})");
                    break;
                default:
                    result = pkb.GetStmtsFollowingT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetStmtsFollowingT({intParameter})");
                    break;
            }
        }
        else
        {
            switch (followed)
            {
                case "":
                    break;
                case "If":
                    result = pkb.GetFollowedByIfsT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByIfsT({intParameter})");
                    break;
                case "While":
                    result = pkb.GetFollowedByWhilesT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByWhilesT({intParameter})");
                    break;
                case "Call":
                    result = pkb.GetFollowedByCallsT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByCallsT({intParameter})");
                    break;
                case "Assign":
                    result = pkb.GetFollowedByAssignsT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByAssignsT({intParameter})");
                    break;
                default:
                    result = pkb.GetFollowedByStmtsT(intParameter);
                    //Console.WriteLine($"invoked pkb.GetFollowedByStmtsT({intParameter})");
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
            stringOutput += "," + element;
        }
        return stringOutput;
    }
}
