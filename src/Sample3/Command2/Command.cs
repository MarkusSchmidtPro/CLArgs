using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs.Sample3.Command2
{
    internal class Command : CommandBase2<CommandParameters>
    {
        protected override void OnExecute(CommandParameters parameters)
        {
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(CommandParameters.Option1)}={parameters.Option1}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option2)}={parameters.Option2}");
            Console.WriteLine($"\t{nameof(CommandParameters.Option3)}={parameters.Option3}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}