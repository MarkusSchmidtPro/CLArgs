using System;
using Level2.CommandBaseSample.SayHello;
using MSPro.CLArgs;



namespace Level2.CommandBaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            
            Command cmd = new SayHello.Command();
            cmd.Execute(arguments);
            
            Console.WriteLine("<<< End Main()");
        }
    }
}
