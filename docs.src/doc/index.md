# The Command-Line Mission

I have been using many different [command-line packages](competition.md) but none of them satisfied my needs. 

## Plug-In concept for Verbs

I wanted to have to ability to automatically resolve new verbs. Implement the verb's functionality in its own Assembly and use it without changing or adding something in my `void Main()`. 
[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat)

## Support multiple Verbs 

I wanted to support multiple verbs, to support commands and subcommands, like `SayHello`, `SayHello Germany`, ... .

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample02.Verbs/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample02.Verbs)

## Parameter class that support sub-classes and inheritance

```csharp
class Parameters : BaseParameters
{
    [OptionSet]
    public Connection DbConnection { get; set; }

    [OptionDescriptor("DatabaseTableName", "t", Required = false)]
    public string DatabaseTableName { get; set; }
}
```
[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample03.Options/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample03.Options)


## Minimum code while having maximum flexibility

Zero Code: `Commander.ExecuteCommand(args);` - if you want.

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat)


## Static and dynamic (dependend) default values

I wanted to have static default values for arguments (during compile-time, by annotation) and also dynamic defaults, resolved during run-time when a default value may depend on another (provided) value.

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault)

## What's next?

* [Basics](basics.md)
* [See all examples](https://github.com/msc4266/CLArgs/tree/master/samples/)
* [Get started with the Command-Line Runner](https://github.com/msc4266/CLArgs/tree/master/CommandRunner)