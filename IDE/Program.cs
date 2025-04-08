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
        // string symbols = "assign a; variable v;";
        // string query = "Select a such that Modifies (a, v) with v.varName=\"x\""; 
        // var queryLexer = new QueryLexer();
        // var queryPreprocessor = new QueryPreprocessor();
        // var queryParser = new QueryParser.QueryParser(queryLexer, queryPreprocessor);
        // queryParser.ParseQuery(symbols + query);

        //if (args.Length != 1)
        //{
        //  Console.WriteLine($"Number of artguments should be 1. Is {args.Length}.");
        //  return;
        //}

        //CodeParser parser = new CodeParser(args[0]);
        CodeParser parser = new CodeParser("C:\\Users\\A\\Source\\Repos\\ATS-Project\\IDE\\simple1.txt");
        if (!parser.ReadFile())
        {
            Console.WriteLine($"File {args[0]} contains nothing.");
            return;
        }

        if (!parser.Parse())
        {
            //Console.WriteLine($"File {args[0]} does not contain parsable code.");
            return;
        }
        else Console.WriteLine("READY");
    }
}
