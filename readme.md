# CLArgs - A dotnet command-line interpreter

Console Applications are simple:  `static void Main(string[] args)` ..  and go! However, getting arguments from a command-line can be complex. Simply run `dotnet build --help` and see! 

>`CLArgs`  turns your console application 
>​	into a **modern command-line application**
>​	with **minimal coding effort**
>​	while providing **maximum flexibility** and **extensibility**.

## Gettings started 

* For a first *example* and the *feature list* read on, scroll down.

* [Modern Console Application Design](doc/index.md)
* [The Mission - what I wanted to accomplish is **what you get**](doc/mission.md)
* [The four levels of using CLArgs](doc/fourLevels.md)
* [All samples on GitHub](../../samples).

## Example

`c:\ > YourApp.exe verb --fileName="myfile.csv" --target=XML`

```csharp
class Program
{
    /// <summary>
	/// 	Run your app: 
    ///		parse command-line and execute your Command
	/// </summary>
    static void Main(string[] args)
    {
        Arguments arguments = CommandLine.Parse(args);
        Console.WriteLine($"Command-Line: '{arguments.CommandLine}'");
        ICommand cmd = new MyConverter();
        cmd.Execute(arguments);
    }
}

/// <summary>
/// Define your command's parameters - supported command-line arguments.
/// </summary>
class MyParams
{
    [CommandLineOption("fileName", Required = true)]
    public string FileName { get; set; }

    [CommandLineOption("target", Required = false, Default = "JSON")]
    public string Target { get; set; }
}

/// <summary>
/// 	Represents your application's functionality.
/// </summary>
/// <remarks>
/// 	The command-line arguments have been turned into an object
/// 	of type MyParams.
/// <y/remarks>
class MyConverter : CommandBase<MyParams>
{
    protected override void OnExecute(MyParams p)
    {
        Console.WriteLine(">>> Start Functionality");
        Console.WriteLine($"fileName='{p.FileName}'");
        Console.WriteLine($"target  ='{p.Target}'");
        Console.WriteLine("<<< End Functionality");
    }
}
```

That is basically how you would eventually use `CLArgs`.

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