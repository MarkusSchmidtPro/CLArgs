---
description: Command Line Argument Parser - modern Console Application made easy
---

# CLArgs - Read Me

Console Applications are simple: `static void Main(string[] args)` .. and go! However, getting arguments from a command-line can be complex. Simply run `dotnet build --help` and see!

> `CLArgs` turns your console application ​ into a **modern command-line application** ​ with **minimal coding effort** ​ while providing **maximum flexibility** and **extensibility**.

`c:\ > YourApp.exe --country=Germany --count=3`

```csharp
internal static class Program
{
    private static void Main(string[] args)
    {
        // Parse the command-line
        Arguments arguments = CommandLineParser.Parse(args);
        // Create a command (that contains the functionality)
        var cmd = new HelloWorldCommand();
        // Let CLArgs convert the arguments and run the command.
        cmd.Execute(arguments);
    }
}

internal class HelloWorldParameters
{
    [OptionDescriptor("country", "c", Required = true)]
    public string Country { get; set; }

    [OptionDescriptor("count", Required = false, Default = 1)]
    public int Count { get; set; }
}

internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
{
    protected override void Execute(HelloWorldParameters ps)
    {
        for (int i = 0; i < ps.Count; i++)
            Console.WriteLine($"Hello {ps.Country}!");
    }
}
```

See [Simple-As-That Source-Code](samples/Sample01.SimpleAsThat/Program.cs) / [Sample Project](samples/Sample01.SimpleAsThat) / [All Samples](samples).

=&gt; [Continue reading documentation](https://github.com/msc4266/CLArgs/wiki)

## Feature List

* A single line of code does the trick: `Commander.ExecuteCommand( args);`
* Unlimited number of _Verbs_
* Plug-In concept: Automatic _Command_ resolution based on _Verbs_
* Different _argument-sets_ for each Command
  * Command Parameter classes support 
    * Sub-Classes \(inheritance\) 
* Flexible argument _validation and completion_
* Clean error-handling to report _all_ errors instead of only the first one
  * When using a console application, 

    It can be frustrating to see "_Param1 is missing_" and 

    once you fixed it you get: "_Param2 is missing_".

  * `CLArgs` reports all errors in a single run.
* Support for command and argument help-text
  * Help-Texts and argument definitions can be loaded from files, from Resources or they can be build-in by code or a combination of these. 
  * This includes support for localized help-messages.
* Support for custom property types and enums in your parameter classes
* Support converting custom value converters
  * to convert any command-line string-value into any Type
* Dynamic default values \(not only static, like True, "abc"\)
  * Including depend default values, e. g. on other values
* ...

Markus Schmidt \(PRO\), Munich \(DE\), 2020-07-10
