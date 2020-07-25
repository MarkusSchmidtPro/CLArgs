using System;
using System.Collections.Generic;
using MSPro.CLArgs;



namespace Level2.DynamicDefault
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            var cmd = new DynamicDefaultCommandBase();
            cmd.Execute(arguments);
            Console.WriteLine("<<< End Main()");
        }



        private class DynamicDefaultCommandBase : CommandBase<CommandParameters>
        {
            protected override void OnResolveProperties( CommandParameters ps, HashSet<string> unresolvedPropertyNames)
            {
                // Check if a property's name is in the list of unresolved
                if (!unresolvedPropertyNames.Contains(nameof(CommandParameters.EndDate))) return;
                
                Console.WriteLine($"Unresolved {nameof(CommandParameters.EndDate)}");
                ps.EndDate = ps.StartDate.AddDays(7);
            }

            protected override void OnExecute(CommandParameters p)
            {
                Console.WriteLine($"Date Range: {p.StartDate:d}..{p.EndDate:d}");
            }
        }


 
        private class CommandParameters
        {
            [OptionDescriptor("StartDate", Required = true)]
            public DateTime StartDate { get; set; }

            [OptionDescriptor("EndDate", Required = false)]
            public DateTime EndDate { get; set; }
        }
    }
}