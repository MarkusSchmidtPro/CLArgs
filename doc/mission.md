# The Mission

I wanted to have a package that supports all [the features listed here](../readme.md).

I wanted to have to ability to automatically resolve a new verb. Implement the verb's functionality in its own Assembly and use it without changing or adding something in my `void Main()` - [Plugin Concept](Level1/verbsWithComposition.md). 

I wanted to support multiple verbs, to support commands and subcommands, like `SayHello`, `SayHello Germany`, ... .

I wanted *Verbs* to become *Commands*. A Command implements a functionality and it should completely stand for itself - no dependency. Extending my console app should be as easy as adding a new Command class.

```csharp
[Export("HelloWorld", typeof(ICommand))]
public class Command : CommandBase<CommandParameters>
{
    protected override void OnExecute(CommandParameters parameters)
    {
		// Functionality ...
	}
}

public class CommandParameters
{
    [CommandLineOption( name:"DatFile", Required = true)]
    public string DatFilename { get; set; }

    [CommandLineOption( name:"Profile")]
    public string ProfileName { get; set; }
}
```

`c.\ > MyApp HelloWorld --DatFile='abc.dat' --Profile=Profile1"`

I wanted to have static default values for arguments (during compile-time, by annotation) and also dynamic defaults resolved during run-time when a default value may depend on another (provided) value.

```csharp
void OnResolveOptions(CommandParameters ps, List<string> unresolvedOptionNames)
{
	if (unresolvedOptionNames.Contains(nameof(ProfileName)))
    	ps.ProfileName = Path.GetFileNameWithoutExtension(ps.DatFilename);
}
```

## What's next

* [The four levels of using CLArgs](fourLevels.md)
* [Directly to Level 2 - see how this Mission was accomplished](Level2\index.md)
* [Level 1 - Basic](Level1\index.md)