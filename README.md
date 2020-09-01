# CLArgs - A .NET command-line interpreter

*CLArgs* is a *NuGet Package* to enable your .NET Console Application to support command-line arguments.

> `CLArgs`  turns your .NET Console Application 
> 	into a **modern command-line application**
> 	with **minimal coding effort**
> 	while providing **maximum flexibility** and **extensibility**.

*CLArgs* supports the following command-line schema: `[Verbs] [Options] [Targets]`.

*CLArgs* parses the command-line and turns *Verbs* into *Commands* and *Options* into parameter *Objects* which are passed to the command.

``` csharp
using MSPro.CLArgs;

class HelloWorldCommand : CommandBase<HelloWorldParameters>
{
	protected override void Execute(HelloWorldParameters ps)
	{
		for (int i = 0; i < ps.Count; i++)
			Console.WriteLine($"Hello {ps.Country}!");
	}
}

internal class HelloWorldParameters
{
	[OptionDescriptor("country", "c", Required = true)]
	public string Country { get; set; }

	[OptionDescriptor("count", Required = false, Default = 1)]
	public int Count { get; set; }
}
```

Using *CLArgs* in your Console Application is simple as that:

```csharp
var cmd = new HelloWorldCommand();
cmd.Execute(CommandLineParser.Parse(args));

> Command-Line: --country=Germany --count=3
Hello Germany!
Hello Germany!
Hello Germany!
```
See [Simple-As-That Source-Code](samples/Sample01.SimpleAsThat/Program.cs) / [Sample Project](samples/Sample01.SimpleAsThat) / [All Samples](samples)

## Installation

Simply add the latest `MSPro.CLArgs` [NuGet package](https://www.nuget.org/packages/MSPro.CLArgs) to your project.

![image-20200810090003001](readme.assets/image-20200810090003001.png)

### What's Next?

The example from above is only one way of using *CLArgs*. Command-Line applications require flexibility and extensibility. See the feature list below what you can expect to get and [continue reading documentation](https://msc4266.github.io/CLArgs/) on github.io to see more. In case of any question, feel free to send me an e-mail.

# Feature List

* A single line of code does the trick: `Commander.ExecuteCommand( args);`
* Unlimited number of *Verbs*
* **Plug-In concept: Automatic *Command* resolution based on *Verbs***
* Different *argument-sets* for each Command
  * Sub-Classes (inheritance) for parameter objects

See *[Command-Runner](https://github.com/msc4266/CLArgs/tree/master/CommandRunner)* for an extensible skeleton to use CLArgs.

* Flexible argument *validation and completion*
* Clean error-handling to report *all* errors instead of only the first one
  * When using a console application, 
    It can be frustrating to see "*Param1 is missing*" and 
    once you fixed it you get: "*Param2 is missing*".
  * `CLArgs` reports all errors in a single run.
* Support for command and argument help-text
  * Help-Texts and argument definitions can be loaded from files, from Resources or they can be build-in by code or a combination of these. 
  * This includes support for localized help-messages.
* Support for custom property types and enums in your parameter classes
* Support converting custom value converters
  * to convert any command-line string-value into any Type
* Dynamic default values (not only static, like True, "abc")
  * Including depend default values, e. g. on other values
* ...

<sub>Markus Schmidt (PRO), Munich (DE), 2020-07-10</sub>
