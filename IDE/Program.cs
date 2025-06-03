using System.Runtime.CompilerServices;
using Atsi.Structures.PKB.Explorer;
using Atsi.Structures.PKB;
using IDE.Parser;
using IDE.PQLParser;

namespace IDE;

public class Program
{
    static void Main(string[] args)
    {
        // odkomentowac do testowania tym śmiesznym narzędziem z ceza
        if (args.Length != 1)
        {
            Console.WriteLine($"Number of artguments should be 1. Is {args.Length}.");
            return;
        }
        var filePath = args[0];
        // do tąd

        // zakomentowac do testowania tym śmiesznym narzędziem z ceza
        
        // var basePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\";
        // var filePath = $"{basePath}SIMPLE.txt";
        
        // do tąd

        CodeParser parser = new CodeParser(filePath);

        if (!parser.ReadFile())
        {
            Console.WriteLine($"File {filePath} contains nothing.");
            return;
        }

        if (!parser.Parse())
        {
            Console.WriteLine($"File {filePath} does not contain parsable code.");
            return;
        }
        else Console.WriteLine("READY");


        QueryParser queryParser = new QueryParser();
        // odkomentowac do testowania tym śmiesznym narzędziem z ceza
        while(true)
        {
            var declarations = Console.ReadLine();
            var query = Console.ReadLine();
            var response = queryParser.ParseQuery(declarations + query);
            // jak się poprawi poniższe TODO to tą linię będzie można usunąć
            var parsed_response = string.Join(",", response.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()));
            Console.WriteLine(string.IsNullOrEmpty(parsed_response) ? "none" : parsed_response);
        }
        // do tąd

        // zakomentowac do testowania tym śmiesznym narzędziem z ceza
        
        // var queryLines = File.ReadAllLines($"{basePath}test2_queries.txt");

        // for (int i = 0; i < queryLines.Length; i += 3)
        // {
        //     if (i + 2 >= queryLines.Length)
        //         break; // zapobiega wyjściu poza zakres przy niepełnej grupie

        //     string declarations = queryLines[i];
        //     string query = queryLines[i + 1];
        //     //string expected = queryLines[i + 2];                  Expected ale tylko dla pliku test2_simple.txt
        //     //Console.WriteLine($"Expected: {expected}");
  
        //     // TODO: response jest do sprawdzenia czy nie robi jakiś dziwnych rzeczy
        //     // zwracanie pustego stringa zamiast "none"
        //     // zwracanie 0 zamiast "none" - 0 w ogóle nigdy nie powinno byś zwracane
        //     // sprawdzenie żeby nie było jakiś przecinków na końcu itp.
        //     // odpowiedź złożona powinna wyglądać na przykład "2,5,7,8"
        //     string response = queryParser.ParseQuery(declarations + query);
        //     // najlepiej żeby parser zwracał już odpowiednio przygotowany string do wypisania, żeby tutaj już go nie parsować
        //     Console.WriteLine(response);
        //     Console.WriteLine(new string('-', 40));
        // }
        
        // do tąd
    }
}
