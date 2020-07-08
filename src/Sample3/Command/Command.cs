using System;
using System.Collections.Generic;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs.Sample3.Command
{
    internal class Command : CommandBase2<CommandParameters>
    {
        protected virtual void OnResolveOptions(IEnumerable<string> unresolvedOptionNames, CommandParameters targetInstance, ErrorDetailList errors)
        {
            foreach (string unresolvedOptionName in unresolvedOptionNames)
            {
                Console.WriteLine($"Unresolved Option: {unresolvedOptionName}");
            }
        }

        protected override void OnExecute(CommandParameters parameters)
        {
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(CommandParameters.Option1)}={parameters.Option1}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option2)}={parameters.Option2}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option3)}={parameters.Option3}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option4)}={parameters.Option4}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}