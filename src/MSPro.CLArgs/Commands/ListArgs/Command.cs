using System;
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs.ListArgs
{
    [UsedImplicitly]
    [Command(COMMAND_NAME, 
             HelpText = "CLArgs built-in: Parse command-line and list all recognized arguments."+
                        "|Does not provide any kind of functionality but helps you to check what could be wrong with your commandline arguments."+
                        " In many cases missing or invalid quotations, especially when using Batch files, can drive you nuts. Use this command" +
                        " to 'debug' you arguments.")]
    internal class Command : ICommand
    {
        private const string COMMAND_NAME = "clargs-list";
        public List<OptionDescriptorAttribute> OptionDescriptors { get; }
        
        public void Execute(CommandLineArguments clArgs, Settings settings)
        {
            Console.WriteLine($"Executing command '{COMMAND_NAME}'");
            Console.WriteLine($"CommandLine: '{clArgs.CommandLine}'");
            Console.WriteLine("");
            
            Console.WriteLine($"{clArgs.Args.Length} argument in args[]");
            for (int i = 0; i < clArgs.Args.Length; i++)
            {
                Console.WriteLine($"args[{i:d2}]: '{clArgs.Args[i]}'");
            }
            Console.WriteLine("");
            

            Console.WriteLine($"Verb-Count: {clArgs.Verbs.Count}");
            for (int i = 0; i < clArgs.Verbs.Count; i++)
            {
                Console.WriteLine($"Verb {i:d2}: '{clArgs.Verbs[i]}'");
            }
            Console.WriteLine("");
            
            Console.WriteLine($"Options-Count: {clArgs.Options.Count}");
            for (int i = 0; i < clArgs.Options.Count; i++)
            {
                Console.WriteLine($"Option {i:d2}: {clArgs.Options[i].Key}='{clArgs.Options[i].Value}'");
            }
            Console.WriteLine("");
            
            Console.WriteLine($"Targets-Count: {clArgs.Targets.Count}");
            for (int i = 0; i < clArgs.Targets.Count; i++)
            {
                Console.WriteLine($"Targets {i:d2}: '{clArgs.Targets[i]}'");
            }
            Console.WriteLine("--- DONE! ---");
        }
    }
}