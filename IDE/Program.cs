using System.Runtime.CompilerServices;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.PKB;
using IDE.Parser;
using IDE.PQLParser;
using IDE.QueryParser;

namespace IDE;

internal class Program
{
    static void Main(string[] args)
    {
        var queryLexer = new QueryLexer();
        var queryPreprocessor = new QueryPreprocessor();
        var queryEvaluator = new QueryEvaluator();
        var queryParser = new QueryParser.QueryParser(queryLexer, queryPreprocessor, queryEvaluator);
        String output = queryParser.ParseQuery(MockQueryData.queries[4].Item1 + MockQueryData.queries[4].Item2);
        Console.WriteLine(output);

        //if (args.Length != 1)
        //{
        //  Console.WriteLine($"Number of artguments should be 1. Is {args.Length}.");
        //  return;
        //}

        //CodeParser parser = new CodeParser(args[0]);
        // CodeParser parser = new CodeParser("C:\\Users\\A\\Source\\Repos\\ATS-Project\\IDE\\simple1.txt");
        // if (!parser.ReadFile())
        // {
        //     Console.WriteLine($"File {args[0]} contains nothing.");
        //     return;
        // }

        // if (!parser.Parse())
        // {
        //     //Console.WriteLine($"File {args[0]} does not contain parsable code.");
        //     return;
        // }
        // else Console.WriteLine("READY");
    }
}
