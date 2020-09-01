# Dynamic Default Values

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault)

In most cases, it is enough to define _static_ default values, which are known during compile-time.

```csharp
[OptionDescriptor("count", Required = false, Default = 1)]
```

However, there are use-cases when you would consider dynamic default values or when a default value depends on another option value. Imagine your app takes two arguments:

`StartDate` \[mandatory\] and `EndDate`\[optional\]

If `EndDate` is not provided in the command-line, it should be `Startdate + 7 days`. In that case, you cannot define a static default value for `Enddate` because it depends on another value.

## Resolve Properties

To accomplish this, override the `BeforeExecute()` method in your Command.

```csharp
class DateRangeCommand : CommandBase<FromToCommandParameters>
{
    protected override void BeforeExecute(
        FromToCommandParameters ps,
        HashSet<string> unresolvedPropertyNames,
        ErrorDetailList errors)
    {
        if (unresolvedPropertyNames.Contains(nameof(ps.EndDate)))
        {
            ps.EndDate = ps.StartDate.AddDays(7);
        }
    }
}
```

## Parameter Validation

You would also use this method to perform Parameter validations: static and dynamic validations:

```csharp
protected override void BeforeExecute(
    FromToCommandParameters ps,
    HashSet<string> unresolvedPropertyNames,
    ErrorDetailList errors)
{
    // Check K.O. criteria: do not run command if fail
    DateTime minStartDate = new DateTime(2020, 1, 1);
    if (ps.StartDate < minStartDate)
    {
        errors.AddError(nameof(ps.StartDate),
            $"{nameof(ps.StartDate)} must be greater or equal to
            {minStartDate:d}, current value: {ps.StartDate:d}");
    }
```

## What's next

* [Value conversion - Option \(string\) Parameter value](convertValues.md)
* [Error-Handling](errorHandling.md)

### Additional throughs

I decided to have one single method `OnResolveProperties` to resolve all properties over something like `[OptionDescriptor( "EndDate", Default => myFunc()]` and over single property resolution like `resolveProperty( string propertyName, ..)`. The reason is: when the resolve property method is called, you can never know which other properties have been resolved. How can you know `StartDate` has got a value when `EndDate` is resolved. How can you guarantee, `StartDate` is resolved before `EndeDate`? You can't!

With single property resolution you have no control over the sequence of resolution.

