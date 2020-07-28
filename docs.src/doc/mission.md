# The Mission

I wanted to have a package that supports all [the features listed here](../readme.md).

I wanted to have to ability to automatically resolve a new verb. Implement the verb's functionality in its own Assembly and use it without changing or adding something in my `void Main()`. 

I wanted to support multiple verbs, to support commands and subcommands, like `SayHello`, `SayHello Germany`, ... .

I wanted *Verbs* to become *Commands*. A Command implements a functionality and it should completely stand for itself - no dependency. Extending my console app should be as easy as adding a new Command class.

```csharp
[Command("HelloWorld")]
internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
{
    protected override void Execute(HelloWorldParameters ps)
    {
        for (int i = 0; i < ps.Count; i++)
            Console.WriteLine($"Hello {ps.Country}!");
    }
}

internal class HelloWorldParameters
{
    [OptionDescriptor("country", "c", Required = true)]
    public string Country { get; set; }

    [OptionDescriptor("count", Required = false, Default = 1)]
    public int Count { get; set; }
}
```

`c:\ > MyApp --country=Germany --count=3`

I wanted to have static default values for arguments (during compile-time, by annotation) and also dynamic defaults resolved during run-time when a default value may depend on another (provided) value.

```csharp
void OnResolveOptions(CommandParameters ps, List<string> unresolvedOptionNames)
{
	if (unresolvedOptionNames.Contains(nameof(ProfileName)))
    	ps.ProfileName = Path.GetFileNameWithoutExtension(ps.DatFilename);
}
```

## What's next

* 