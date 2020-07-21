# Level 1 - Basic

The very first step when you use `CLArgs` is to parse the command-line: `CommandLine.Parse(args)`. This will return and `Arguments`object containing all *Verbs* and *Options* from the command-line.

```csharp
using MSPro.CLArgs;

static void Main(string[] args)
{
	Arguments arguments = CommandLine.Parse(args);

	//
	// Functionality: Display arguments
	// 
	Console.WriteLine(">>> Start Functionality");
	Console.WriteLine($"Command-Line: {arguments.CommandLine}");

	for (int i = 0; i < arguments.Verbs.Count; i++)
	{
		string verb = arguments.Verbs[i];
		Console.WriteLine($"Verb[{i}] = '{verb}''");
	}

	foreach (KeyValuePair<string,OptionTag> option in arguments.Options)
	{
		Console.WriteLine($"Options[{option.Key}] = '{option.Value}''");
	}
	
	Console.WriteLine("<<< End Functionality");
}
```

This is what you get: two *Verbs* and three *Options* (starting with '--'):

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

Simple as that, do with the parsed information whatever you want to do. *Options* are a key-value list and all values are strings as parsed they have been from the command-line.

## What's next

* [Verbs and Options](doc/verbsAndOptions.md)
* [Plugin concept with Verbs and Microsoft Composition](doc/verbsWithComposition.md)
* [Level 2](doc/level2.md)