# CLArg - A dotnet command-line interpreter

C-Sharp Console Applications are simple:  `static void Main(string[] args)` ..  and go!

Getting arguments from a command-line can be complex. `CLArgs` takes this complex part to keep your console application simple.

> I thought it was time to reinvent the wheel!
>
> Not the "Flint*stone"* wheel, not the wooden one and not the rubber one. There are so many command-line interpreters out there. I wanted to invent the wheel with air inside, where you can chose the right pressure you need - one fits all.
>
> Let's reinvent the way how to build command-line apps.

## Four levels of using it

There are four levels of using `CLArgs`

### Level 1 - Basic

A bit more than just `string[] args`. 

Simply parse your command-line into `Arguments` - *Verbs* and *Options*:

```
c:\ > YourApp --fileName="myfile.csv" --target=XML
Arguments arguments = Commander.ParseCommandLine(args);
```

### Level 2 - Standard

Parse the command-line into a typed object and pass it to your code. This level supports (multiple)  *Verbs* and *Options*, and this is probably the right choice for most of you. I am using `CLArgs` at this level.

### Level 3 - Advanced

All those who are not happy with *Level 2*, who need more flexibility or who have other special requirements: a) let me know, what is missing and then enter b) the Expert level. `CLArgs` has several hooks and /or extension points where you can integrate your code.

### Level 4 - Expert

Check out the source-code and use it for your convenience. Don't forget to [let me know](mail:markus@markusschmidt.pro) what you would make better.



Instead of `void Main( string[] args)` you will use `void MyApp( MyParams p)` where `MyParams` is your class that contains all application parameters which were provided as a command-line. `CLArg` will take care that the arguments provided in the command-line will flow to your `MyParams` object.

Simple as that: Imagine you have a command-line (console) application that converts a CSV file into whatever you specify as the *Target*: JSON or XML. 

`c:\ > YourApp --fileName="myfile.csv" --target=XML`


```csharp
class Program
{
    static void Main(string[] args)
    {
        Arguments arguments = Commander.ParseCommandLine(args);
        Console.WriteLine($"Command-Line: ''{arguments.CommandLine}''");
        ICommand cmd = new MyConverter();
        cmd.Execute(arguments);
    }
}

class MyParams
{
    [CommandLineOption("fileName", Required = true)]
    public string FileName { get; set; }

    [CommandLineOption("target", Required = false, Default = "JSON")]
    public string Target { get; set; }
}

// MyConverter implements the functionality 
// and it gets the arguments from the command-line as a typed object.
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

Check`Samples\SuperSimple` project and see the output:

```bash
Command-Line: '--fileName=myfile.csv --target=XML'
>>> Start Functionality
fileName='myfile.csv'
target='XML'
<<< End Functionality
```

Right: the *super simple sample* is "just a wheel". We wanted to re-invent something better. If the *super simple sample* works for you, great! Otherwise, go ahead, and see *[How it works in detail](doc/index.md)* and some [Use-Cases and examples](doc/UseCases/index.md).

## Feature List

* Unlimited number of *Verbs*
* Plug-In concept: Automatic Command resolution based on *Verbs*
* Different *argument-sets* for each command
* Flexible arguments *validation and completion*
* Clean error-handling tp report *all* errors instead of only the first one
  * When using a console application, 
    It can be frustrating to see "*Param1 is missing*" and 
    once you fixed it you get: "*Param2 is missing*".
  * `CLArgs` reports all errors in a single run.
* Support for command and argument help-text
  * Help-Text and even argument definitions can be loaded 
    * from files or from Resources or can be build-in in code,
      or a combination of these.
  * Support for localized messages.
* Support for custom property types and enums in your parameter classes
* Dynamics default values (not only static, like "abc")
  * incl. default dependencies (e. g. on other values)
<hr/>
<sub>Markus Schmidt (PRO), Munich (DE), 2020-07-10</sub>