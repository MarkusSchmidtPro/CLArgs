---
description: Documentation
---

# CLArgs - A dotnet command-line parser

_CLArgs_ is a full featured, easy to use command-line parser for _dotnet_ applications.

Reference the [_CLArgs_ NuGet Package](https://www.nuget.org/packages/MSPro.CLArgs) in your console project   
and modify your `void Main()`method like this:

```csharp
private static void Main(string[] args)
{
  Commander.ExecuteCommand( args);
}
```

That is all you have to do to get support for 

`Application.exe [Verbs] [Options] [Targets]`

**Verbs** - represent different actions / commands your application can execute.

**Options** - represent the options / parameters which a _Command_ \(verb\) may need to run.

**Targets** - represent addition information, like a list of files.

### Commander

The _Commander_ will [look for _Command_ implementations](command-resolution/) \(classes annotated with `[Command( <CommandName=Veb>)]`and it will execute the _Command_ that matches the _Verb_ provided in that command-line.

Imagine `> YourApp.exe ConverToUTC --option1`. The provided _Verb_ is `ConverToUTC`  and _Commander_ will look for a class annotated with `[Command( "ConverToUTC"].`

### Command

_Commands_ represent the functionality of your application. Your Console application can have one or many _Commands_, even distributed in many Assemblies. The _Verb\(s\)_ - provided in command-line during run-time - define which _Command_ will finally be executed. 

> Note: If you don't provide a _Verb_ in the command-line _Commander_ executes the first _Command_  it can find \(see [Command Resolution](command-resolution/)\). This is perfectly fine for apps which have only one single functionality - as normal console apps have only one single `void main()`.

Normally a _Command_  takes the code that you would have implemented in your `Main` method. However, a _Command_ does not deal with `string[] args` anymore, it executes with [Command Parameters](command-parameters.md).

```csharp
[Command("ConvertToUtc")]
class ConvertToUtcCommand : CommandBase<ConvertToUtcParameters>
{
  protected override void Execute(ConvertToUtcParameters ps)
  {
    // your main method's code
  }
}
```

Check out [the first example on GitHub 'ConvertToUTC'](https://github.com/msc4266/CLArgs/tree/dev/samples/Sample.ConvertToUtc).

### Plug-In Concept

It is important to notice, that you can extend your application at any time by simply adding new _Commands_. _Commands_ are atomic and they have no dependency on the environment where they are running. Just implement another _Command_ class and give it a name \(a _Verb_\). Add the _Command_'s Assembly to your application _bin_ folder. And you're done! With the very next start of your console application your application supports another _Command/Verb_ \(see [Command Resolution](command-resolution/)\).

## What's next

* [Command Parameters](command-parameters.md)
* [Explore the examples and read more about CLArgs](doc/index.md)

