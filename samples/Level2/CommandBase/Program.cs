using System;
using MSPro.CLArgs;



namespace Level2.CommandBaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = Commander.ParseCommandLine(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            
            ICommand cmd = new SayHello.Command();
            cmd.Execute(arguments);
            
            Console.WriteLine("<<< End Main()");
        }
    }
}
