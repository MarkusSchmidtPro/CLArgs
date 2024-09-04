using System;
using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;



namespace ConsoleApp.NET;

internal class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.ConfigureCommands(args);
        builder.Build().Start();
    }
}



[Command("HelloWorld", "A simple command to print a name in the Console Window.")]
public class HelloWorldCommand(IServiceProvider serviceProvider) : CommandBase2<HelloWorldContext>(serviceProvider)
{
    protected override void Execute()
    {
        Console.WriteLine($"Hello {_context.Name}");
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