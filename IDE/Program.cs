using System.Runtime.CompilerServices;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.PKB;
using IDE.Parser;
using IDE.PQLParser;

namespace IDE;

internal class Program
{
    static void Main(string[] args)
    {
        //if (args.Length != 1)
        //{
        //  Console.WriteLine($"Number of artguments should be 1. Is {args.Length}.");
        //  return;
        //}

        //CodeParser parser = new CodeParser(args[0]);
        // CodeParser parser = new CodeParser("C:\\Users\\A\\Source\\Repos\\ATS-Project\\IDE\\simple1.txt");
        
        CodeParser parser = new CodeParser("C:\\Users\\A\\Source\\Repos\\ATS-Project\\IDE\\SIMPLE.txt");

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

        QueryParser queryParser = new QueryParser();
        var queryLines = File.ReadAllLines("C:\\Users\\Filip\\Documents\\studia\\atsi_project\\ATS-Project\\IDE\\test2_queries.txt");

        for (int i = 0; i < queryLines.Length; i += 3)
        {
            if (i + 2 >= queryLines.Length)
                break; // zapobiega wyjściu poza zakres przy niepełnej grupie

            string declarations = queryLines[i];
            string query = queryLines[i + 1];
            //string expected = queryLines[i + 2];                  Expected ale tylko dla pliku test2_simple.txt
            //Console.WriteLine($"Expected: {expected}");
  
            string actual = queryParser.ParseQuery(declarations + query);

            Console.WriteLine(string.IsNullOrWhiteSpace(actual) ? "none" : actual.Trim());
            Console.WriteLine(new string('-', 40));
        }
    }
}
