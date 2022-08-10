using System;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs.ListArgs;

[UsedImplicitly]
[Command(COMMAND_NAME,
    HelpText = "CLArgs built-in: Parse command-line and list all recognized arguments." +
               "|Does not provide any kind of functionality but helps you to check what could be wrong with your commandline arguments." +
               " In many cases missing or invalid quotations, especially when using Batch files, can drive you nuts. Use this command" +
               " to 'debug' you arguments.")]
internal class ListArgsCommand : ICommand2
{
    private const string COMMAND_NAME = "clargs-list";
    private readonly IArgumentCollection _clArgs;



    public ListArgsCommand(IArgumentCollection _clArgs)
    {
        _clArgs = _clArgs;
    }



    public void Execute()
    {
        Console.WriteLine($"Executing command '{COMMAND_NAME}'");
        Console.WriteLine($"CommandLine: '{Environment.CommandLine}'");
        Console.WriteLine("");

        Console.WriteLine($"{_clArgs.Count} argument in args[]");
        for (int i = 0; i < _clArgs.Count; i++)
        {
            Console.WriteLine($"args[{i:d2}]: '{_clArgs[i]}'");
        }

        Console.WriteLine("");


        Console.WriteLine($"Verb-Count: {_clArgs.Verbs.Count()}");
        for (int i = 0; i < _clArgs.Verbs.Count(); i++)
        {
            Console.WriteLine($"Verb {i:d2}: '{_clArgs.Verbs.ElementAt(i)}'");
        }

        Console.WriteLine("");

        Console.WriteLine($"Options-Count: {_clArgs.Options.Count()}");
        for (int i = 0; i < _clArgs.Options.Count(); i++)
        {
            Console.WriteLine($"Option {i:d2}: {_clArgs.Options.ElementAt(i).Key}='{_clArgs.Options.ElementAt(i).Value}'");
        }

        Console.WriteLine("");

        Console.WriteLine($"Targets-Count: {_clArgs.Targets.Count()}");
        for (int i = 0; i < _clArgs.Targets.Count(); i++)
        {
            Console.WriteLine($"Targets {i:d2}: '{_clArgs.Targets.ElementAt(i)}'");
        }

        Console.WriteLine("--- DONE! ---");
    }
}