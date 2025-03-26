using IDE.Parser;

namespace IDE;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine($"Number of artguments should be 1. Is {args.Length}.");
            return;
        }

        CodeParser parser = new CodeParser(args[0]);
        if (!parser.ReadFile())
        {
            Console.WriteLine($"File {args[0]} contains nothing.");
            return;
        }

        if (!parser.Parse())
        {
            Console.WriteLine($"File {args[0]} does not contain parsable code.");
            return;
        }
        else Console.WriteLine("READY");
    }
}
