# CLArg - A dotnet command-line interpreter

C-Sharp Console Applications are simple:  `static void Main(string[] args)` ..  and go!

Getting arguments from a command-line can be complex. `CLArgs` takes this complex part to keep your console application simple.

> I thought it was time to reinvent the wheel!
>
> Not the (Flint) stone wheel, not the wooden one and not the rubber one. There are so many command-line interpreters out there. I wanted to invent the wheel with air inside, where you can chose the right pressure you need - one fits all. 
>
> Let's reinvent the way how to build command-line apps.

## Example

`c:\ > YourApp --fileName="myfile.csv" --target=XML`


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
        Console.WriteLine($"Command-Line: ''{arguments.CommandLine}''");
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
        Console.WriteLine($"fileName='{p.FileName}''");
        Console.WriteLine($"target='{p.Target}''");
        Console.WriteLine("<<< End Functionality");
    }
}
```

That is basically how you would eventually use `CLArgs` on a "low *Level 2*".

## Four levels of using CLArgs

There are four levels of using `CLArgs`

### Level 1 - Basic

Level 1 is a bit more than just `string[] args`.  Simply parse your command-line into `Arguments` and work with *Verbs* and *Options*:

```csharp
c:\> YourApp --fileName="myfile.csv" --target=XML
Arguments arguments = CommandLine.Parse(args);
```

[More a about Level 1](doc/level1.md)

### Level 2 - Standard

Parse the command-line into a typed object and pass it to your Command (as shown in the code above). A Command represents the functionality (that is normally  `void Main`).  However, Commands can be bound to *Verbs* so that one console app can support multiple functionalities with different argument sets.

The *Standard Level* supports (multiple) *Verbs* and *Options*, and it is probably the right choice for most of you. I am using `CLArgs` at this level. [More a about Level 2](doc/level2.md)

> Most other command-line solutions I have seen, work between Level 1 and 2.

### Level 3 - Advanced

All those who are still not happy with *Level 2*, who need more flexibility or who have other special requirements: a) let me know, what is missing and then enter b) Level 3. `CLArgs` has several hooks and /or extension points where you can integrate your code, and you can use the `CLArgs` classes and functions as you want it. [More a about Level 3](doc/level3.md)

### Level 4 - Expert

Check out the source-code and use it for your convenience. Don't forget to [let me know](mail:markus@markusschmidt.pro) what you would make better.

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
* Dynamics default values (not only static, like True, "abc")
  * Including depend default values, e. g. on other values
* ...
<hr/>
<sub>Markus Schmidt (PRO), Munich (DE), 2020-07-10</sub>