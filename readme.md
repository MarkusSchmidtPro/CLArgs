# CLArgs - A dotnet command-line interpreter

Console Applications are simple:  `static void Main(string[] args)` ..  and go! However, getting arguments from a command-line can be complex. Simply run `dotnet build --help` and see! 

>`CLArgs`  turns your console application 
>​	into a **modern command-line application**
>​	with **minimal coding effort**
>​	while providing **maximum flexibility** and **extensibility**.

## Getting started 

* For a first *example* and the *feature list* read on, scroll down.

* [Modern Console Application Design](doc.fx/mission/index.md)
* [The Mission - what I wanted to accomplish is **what you get**](doc.fx/mission/mission.md)
* [All samples on GitHub](Samples).

## Example

`c:\ > YourApp.exe --country=Germany --count=3`

```csharp
/// <summary>
///     The easiest way to use CLArgs.
/// </summary>
/// <remarks>
///     Let the <see cref="Commander" /> automatically
///     resolve all classes in the Entry Assembly
///     which inherit from <see cref="CommandBase{TParam}" /> and which are
///     annotated with a <see cref="CommandAttribute">[Command]</see>-Attribute.<br />
/// </remarks>
internal static class Program
{
	private static void Main(string[] args)
	{
        // Auto Resolve Command("HelloWorld")
        // Turn args into a HelloWorldParameters instance
        // Run HelloWorldCommand.Execute(HelloWorldParameters ps)
		Commander.ExecuteCommand(args);
	}
}


internal class HelloWorldParameters
{
	[OptionDescriptor("country", "c", Required = true)]
	public string Country { get; set; }

	[OptionDescriptor("count", Required = false, Default = 1)]
	public int Count { get; set; }
}


[Command("HelloWorld")]
internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
{
	protected override void Execute(HelloWorldParameters ps)
	{
		for (int i = 0; i < ps.Count; i++)
			Console.WriteLine($"Hello {ps.Country}!");
	}
}
```

See [source-code](Samples/Sample01.SimpleAsThat/Program.cs) / [sample project](Samples/Sample01.SimpleAsThat) / [all samples](Samples).

## Feature List

* Unlimited number of *Verbs*
* Plug-In concept: Automatic Command resolution based on *Verbs*
* Different *argument-sets* for each Command
* Flexible arguments *validation and completion*
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
* Dynamics default values (not only static, like True, "abc")
  * Including depend default values, e. g. on other values
* ...

<sub>Markus Schmidt (PRO), Munich (DE), 2020-07-10</sub>