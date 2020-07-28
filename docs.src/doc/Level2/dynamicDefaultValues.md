# Dynamic Default Values

In most cases, it is enough to define *static* default values. However, there are use-cases when you would consider dynamic default values.

Imagine your app takes two arguments: `StartDate` [mandatory] and `EndDate`[optional]

If `EndDate` is not provided in the command-line, it should be `Startdate + 7 days`. You can<u>not</u> define a static Default value for `Enddate` because it depends on another parameter.

> Don't worry about `void Main()` it is always the same:
>
> ```csharp
> ICommand cmd = new Command();
> cmd.Execute(arguments);
> ```

## Resolve Properties

When  `CLArgs` is doing its magic it must resolve the *Options* which were provided in the command-line and and it must find an appropriate Property in the target object (Command Parameters). `CLArgs` is scanning for `OptionDescriptorAttributes` to find matching properties.

```csharp
[OptionDescriptor("StartDate",  Required = true)]
public DateTime StartDate { get; set; }

[OptionDescriptor( "EndDate", Required = false)]
public DateTime EndDate { get; set; }
```

`CLArgs` assumes that <u>all</u> properties which are annotated with an `OptionDescriptorAttribute` should have got a value: either provided in the command-line or as a static default value.

In the example above, `EndDate` does not have a default value, and in case it was not provided in the command-line, the `EndDate` property remains *unresolved*. 

Before `CLArgs` calls `Command.Execute()` method it calls  `OnResolveProperties()`. This method gets the `CommandParameters` object that was populated with all resolved properties and it gets a list with all unresolved property names.

```csharp
protected override void OnResolveProperties(
    CommandParameters ps,
    List<string> unresolvedPropertyNames)
{
	// Check if a property's name is in the list of unresolved
    if( unresolvedPropertyNames.Contains(
        	nameof(CommandParameters.EndDate)))
    {
	 	Console.WriteLine(
         $"Unresolved {nameof(CommandParameters.EndDate)}");
		ps.EndDate = ps.StartDate.AddDays(7);
    }
}

protected override void OnExecute(CommandParameters p)
{
	Console.WriteLine(
        $"Date Range: {p.StartDate:d}..{p.EndDate:d}");
}
```

```
Command-Line: --StartDate=2020-01-01
>>> Start Main()
Unresolved EndDate
Date Range: 01.01.2020..08.01.2020
<<< End Main()
```

See [source-code](../../samples/DynamicDefault/Program.cs) / [sample project](../../samples/DynamicDefault) / [all samples](../../samples).

## What's next

* [Value conversion - Option (string)  value to property value](convertValues.md)
* Error handling

### Additional throughs

I decided to have one single method `OnResolveProperties` to resolve all properties over something like `[OptionDescriptor( "EndDate", Default => myFunc()]` and over single property resolution like `resolveProperty( string propertyName, ..)`. The reason is: when the resolve property method is called, you can never know which other properties have been resolved. How can you know `StartDate` has got a value when `EndDate` is resolved. How can you guarantee, `StartDate` is resolved before `EndeDate`? You can't!

With single property resolution you have <u>no</u> control over the sequence of resolution.
