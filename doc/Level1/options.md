# Options

Options are (configuration) values that your application requires to execute a certain functionality. 

* *Options* start with a tag, which is by default: / or -- or -
* An *Option* has a tag and a value (tag=name=key).
* Options are the parameters for a functionality (*[Verb](verbsAndOptions.md)*)

`Argument.Options` is a dictionary, where the Option's name is key. 

```
c:\ > MyApp --Opt1=45`

: Code
Arguments arguments = CommandLine.Parse(args);

: Result
arguments.Options["Opt1"] = "45"
```

* An Option is unique by its name.
  * The Option's name is the *Tag* (Name=Key=Tag) that was provided in the command-line
  * If the same Option is provided multiple times in the command-line, 
    the <u>last one wins</u> rule is applied. 
* An *Option* value is always of type *string*, as it was parsed from the command-line.
* An *Option* value can be null (e. g. `--Flag`)

## Update or Add Options

You can update or add options when required: use the `Arguments.Upsert` (*Upsert = **UP**date or in**SERT***) for this purpose.

```csharp
const string OPTION_NAME = "fileName";

Arguments arguments = CommandLine.Parse(args);

// Check if the 'fileName' option exist - was provided in the command-line
bool fileNameProvided = arguments.Options.ContainsKey(OPTION_NAME);
Console.WriteLine($"Option '{OPTION_NAME}' was provided in the command-line: {fileNameProvided}");

// Set a default Option if not provided
// Upsert = Update or Insert = Update or Add 
if (!fileNameProvided)
    arguments.UpsertOption(OPTION_NAME, "default.txt");
```

See [source-code](../../samples/Level1/Sample1.Options/Program.cs) / [sample project](../../samples/Level1/Sample1.Options) / [all samples](../../samples).


## Preview to Level 2

You will see in [Level2](../Level2/index.md) that Option values can have types. For now, and as long as you work with the `Arguments` object only, Option values are: *string*, and an Option name/key and its tag is the same. Also in Level2 you will then learn that an Option has *one* name but it can have multiple *tags*: Name="filename", tags= "--f", "--fn", "--fName".    

More to say about Options? Yes, a lot.

## What's next

* [Level 2](../level2.md)
* [Verbs and Options](verbsAndOptions.md)