# Welcome to CLArgs documentation

Master your Modern Console Apps made easy.

* Create a `Command` that implements the functionality
* Define the Command `Parameters`
* and let *CLArgs* convert the command-line arguments and call the Command.

> <span style="color:darkred">**TIP:**</span> Check-Out the [Console App Skeleton](https://github.com/msc4266/CLArgs/tree/master/CommandRunner) to get a complete skeleton for your Console App.

```csharp
static class Program
{
	static void Main(string[] args)
	{
		Arguments arguments = CommandLineParser.Parse(args);
		var cmd = new HelloWorldCommand();
		cmd.Execute(arguments);
	}
}

class HelloWorldParameters
{
	[OptionDescriptor("country", "c", Required = true)]
	public string Country { get; set; }

	[OptionDescriptor("count", Required = false, Default = 1)]
	public int Count { get; set; }
}

class HelloWorldCommand : CommandBase<HelloWorldParameters>
{
	protected override void Execute(HelloWorldParameters ps)
	{
		for (int i = 0; i < ps.Count; i++)
			Console.WriteLine($"Hello {ps.Country}!");
	}
}
```

This is a very basic example, of what you could do with CLArgs.

## What's next

* [Explore GitHub and the ReadMe to see all features](https://github.com/msc4266/CLArgs)
* [Get the NuGet Package](https://www.nuget.org/packages/MSPro.CLArgs)
* [Explore the examples and read more about CLArgs](doc/index.md)