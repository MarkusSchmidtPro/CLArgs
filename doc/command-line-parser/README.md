---
description: Parsing the Command-line arguments
---

# Command Line Parser

The very first step in any functionality is always: parsing the `string args[]` provided in the command-line. Even if you normally won't see this happen, it always takes place behind the scenes.

There is one class that does this job: `CommandLineParser`. This class parses the command-line and creates _Verbs, Options and Targets._

```csharp
private static void Main(string[] args)
{
    CommandLineArguments arguments 
        = CommandLineParser.Parse( args);
    
    foreach (string verb in arguments.Verbs) ..
    foreach (Option option in arguments.Options) ..
    foreach (string target in arguments.Targets) ..
}
```

See [ParseOnly example](https://github.com/msc4266/CLArgs/blob/master/samples/Sample00.ParseOnly/Program.cs) / [ParseOnly project](https://github.com/msc4266/CLArgs/blob/master/samples/Sample00.ParseOnly)

### Case-sensitive or ignore case

You may wonder, how to control whether _Option_ parsing is case-sensitive or if it will ignore cases? The answer is simple: **The `CommandLineParser` always parses case-sensitive.** 

The reason for this behavior is, the _Parser_ parses the command-line, it does **not** interpret the provided values. So, for the Parser _FILENAME_ and _filename_ are different strings, which result in different Options \(see [ParseOnly project](https://github.com/msc4266/CLArgs/blob/master/samples/Sample00.ParseOnly)\).

```text
MayApp.exe verb2 VERB2 --FILENAME='def' --fileName='c:\\myfile.csv'

CommandLineArguments:
Verb='verb2'
Verb='VERB2'
Options[FILENAME] = 'def'
Options[fileName] = 'c:\myfile.csv'
```

> Of course, this is just half the story. However, if you' re using the **`CommandLineParser`** you should be aware of this 'sensitive behavior'. You will find the rest of the story and how to configure `IgnoreCase` in the [Option resolution and argument conversion](../the-commander/) section.

### Settings - to control Option recognition

By providing a `Settings`object you can control how _Options_ are being recognized. 

```csharp
CommandLineArguments arguments = CommandLineParser.Parse( 
    args,
    new Settings
    {
        OptionsTags = new []{ '-', '/'} ,
        OptionValueTags = new [] {' ', ':', '='}
    }
```

`OptionTags` - define the tokens which mark an _Option_ \(note: '--' will also be recognized as an Option tag\).

_`OptionValueTags` -_  define the tokens that split an _Option_ name from its value

With these default values, you can specify _Options_ like that:

```text
--Option1
-Option1
/Option2='abc'
/Option2:'abc'
/OPtion2 'abc' --Option3=123 -boolOption=true
```

> Note: If you use `Commander.ExecuteCommand()` you can also provide a `Stettings` object to control command-line parsing \(see [Command Resolution](../command-resolution/)\).

