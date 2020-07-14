using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs.Sample3.Command
{
    internal class Command : CommandBase2<CommandParameters>
    {
        protected override void OnResolveProperties(CommandParameters parameters, List<string> unresolvedPropertyNames)
        {
            //foreach (var pi  in unresolvedPropertyNames)
            //{
            //    Console.WriteLine($"Unresolved Properties (not provided in command-line): {pi}");
            //}

            if( unresolvedPropertyNames.Contains( nameof(CommandParameters.StartDate)))
            {
                parameters.StartDate = DateTime.Now;
            }
        }

        protected override void OnExecute(CommandParameters parameters)
        {
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(CommandParameters.Option1)}={parameters.Option1}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option2)}={parameters.Option2}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option3)}={parameters.Option3}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option4)}={parameters.Option4}");
            Console.WriteLine($"\t{nameof(CommandParameters.StartDate)}={parameters.StartDate}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}