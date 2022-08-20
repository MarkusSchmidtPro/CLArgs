using MSPro.CLArgs;



// -----------------------------------
// Simple example how to use CLArgs V2
// -----------------------------------
class Program
{
    // builder has some functionality that is not used here.
    // We go with the defaults.
    public static void Main(string[] args)
    {
        var builder = CommandBuilder.Create();
        Commander2 commander = builder.Build();
        commander.Execute();
    }
}



[Command("HelloWorld", helpText: "A simple command to print a name in the Console Window.")]
internal class HelloWorldCommand : CommandBase2<HelloWorldContext>
{
    public HelloWorldCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override void Execute()
    {
        Console.WriteLine($"Hello {this.Context.Name}");
    }
}



/// <summary>
///     The context defines and describes the arguments supported by a Command.
/// </summary>
internal class HelloWorldContext
{
    [OptionDescriptor("Name", new[] { "n" },
        Default = "John Doe", Required = false,
        HelpText = "Specify the name of the person to say 'Hello'.")]
    public string? Name { get; set; }
}