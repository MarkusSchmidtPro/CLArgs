# Level 1 - Basic

The very first step when you use `CLArgs` is to parse the command-line: `Commander.ParseCommandLine(args)`. This will return an `Arguments` object containing all *Verbs* and *Options* from the command-line.

```csharp
using MSPro.CLArgs;

static void Main(string[] args)
{
	Arguments arguments = Commander.ParseCommandLine(args);

	//
	// Functionality: Display arguments
	// 
	Console.WriteLine(">>> Start Functionality");
	Console.WriteLine($"Command-Line: {arguments.CommandLine}");

    foreach (string verb in arguments.Verbs)
    {
        Console.WriteLine($"Verb='{verb}'");
    }

	foreach (Option option in arguments.Options)
    {
    	Console.WriteLine($"Options[{option.Tag}] = '{option.Value}'");
	}
	
	Console.WriteLine("<<< End Functionality");
}
```

See [source-code](../../samples/Level1/Sample1/Program.cs) / [sample project](../../samples/Level1/Sample1) / [all samples](../../samples).

### Example

Command-Line with two *Verbs* and three *Options* (starting with '--'):

```batch
>>> Start Functionality
Command-Line: verb1 verb2 --fileName='c:\myfile.csv' --target=XML --lines=7
Verb[0] = 'verb1'
Verb[1] = 'verb2'
Options[fileName] = 'c:\myfile.csv'
Options[target] = 'XML'
Options[lines] = '7'
<<< End Functionality
```

## What's next

* [Verbs and Options](verbsAndOptions.md)
* [Plugin concept with Verbs and Microsoft Composition](verbsWithComposition.md)
* [Level 2](../level2.md)