using System;
using CLArgs.CommandRunner.SayHello;
using MSPro.CLArgs;



namespace CLArgs.CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            
            Command cmd = new Command();
            cmd.Execute(arguments);
            
            Console.WriteLine("<<< End Main()");
        }
    }
}
