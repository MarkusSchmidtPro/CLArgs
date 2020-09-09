# Dynamic Default Values

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault)

In most cases, it is enough to define _static_ default values, which are known during compile-time.

```csharp
[OptionDescriptor("count", Required = false, Default = 1)]
```

However, there are use-cases when you would consider dynamic default values or when a default value depends on another option value. 

Imagine your application takes two arguments:

`StartDate` \[mandatory\] and `EndDate`\[optional\]

If `EndDate` is not provided in the command-line, it should be `Startdate + 7 days`. In that case, you cannot define a static default value for `Enddate` because it depends on another value.

To accomplish this, override the `BeforeExecute()` method in your _Command_.

```csharp
class DateRangeCommand : CommandBase<FromToCommandParameters>
{
    protected override void BeforeExecute(
        TCommandParameters ps,
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

