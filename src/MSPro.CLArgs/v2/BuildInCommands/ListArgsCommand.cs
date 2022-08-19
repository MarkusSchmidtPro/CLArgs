using System;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs.BuildInCommands;

[UsedImplicitly]
[Command(COMMAND_NAME,
    HelpText = "CLArgs built-in: Parse command-line and list all recognized arguments." +
               "|Does not provide any kind of functionality but helps you to check what could be wrong with your commandline arguments." +
               " In many cases missing or invalid quotations, especially when using Batch files, can drive you nuts. Use this command" +
               " to 'debug' you arguments.")]
public class ListArgsCommand : ICommand2
{
    private const string COMMAND_NAME = "clargs-list";
    private readonly IArgumentCollection _arguments;

    public ListArgsCommand(IArgumentCollection arguments)
    {
        _arguments = arguments;
    }



    IOptionCollection ICommand2.CommandOptions { get; } = new OptionCollection();



    public void Execute()
    {
        Console.WriteLine($"Executing command '{COMMAND_NAME}'");
        Console.WriteLine($"CommandLine: '{Environment.CommandLine}'");
        Console.WriteLine("");

        Console.WriteLine($"{_arguments.Count} argument in args[]");
        for (int i = 0; i < _arguments.Count; i++)
        {
            Console.WriteLine($"args[{i:d2}]: '{_arguments[i]}'");
        }

        Console.WriteLine("");


        Console.WriteLine($"Verb-Count: {_arguments.Verbs.Count()}");
        for (int i = 0; i < _arguments.Verbs.Count(); i++)
        {
            Console.WriteLine($"Verb {i:d2}: '{_arguments.Verbs.ElementAt(i)}'");
        }

        Console.WriteLine("");

        Console.WriteLine($"Options-Count: {_arguments.Options.Count()}");
        for (int i = 0; i < _arguments.Options.Count(); i++)
        {
            Console.WriteLine($"Option {i:d2}: {_arguments.Options.ElementAt(i).Key}='{_arguments.Options.ElementAt(i).Value}'");
        }

        Console.WriteLine("");

        Console.WriteLine($"Targets-Count: {_arguments.Targets.Count()}");
        for (int i = 0; i < _arguments.Targets.Count(); i++)
        {
            Console.WriteLine($"Targets {i:d2}: '{_arguments.Targets.ElementAt(i)}'");
        }

        Console.WriteLine("--- DONE! ---");
    }
}