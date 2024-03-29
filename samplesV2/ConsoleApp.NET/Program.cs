﻿using System;
using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;



namespace ConsoleApp.NET;

internal class Program
{
    public static void Main(string[] args)
    {
        CommandHostBuilder.Create(args).Start();
    }
}



[Command("HelloWorld", "A simple command to print a name in the Console Window.")]
public class HelloWorldCommand : CommandBase2<HelloWorldContext>
{
    public HelloWorldCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }



    protected override void Execute()
    {
        Console.WriteLine($"Hello {Context.Name}");
    }
}



/// <summary>
///     The context defines and describes the arguments supported by a Command.
/// </summary>
public class HelloWorldContext
{
    [OptionDescriptor("Name", new[] { "n" },
        Default = "John Doe", Required = false,
        HelpText = "Specify the name of the person to say 'Hello'.")]
    public string? Name { get; set; }
}