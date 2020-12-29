# BeforeExecute\(\) & Parameter Validation

Sample [Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault/Program.cs) / [Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample04.DynamicDefault)

The `CommandBase` class provides several methods that can be overridden. Check the class documentation for more information. One of the most important methods is `BeforeExecute`. 

In the `BeforeExecute`method you can check if there is an _unresolved property_ and/or you can provide [dynamic default values](dynamic-default-values.md).

```csharp
protected override void BeforeExecute(
    TCommandParameters ps,
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

The `HashSet<string> unresolvedProperties` contains all annotated properties which have not been resolved. For example, imagine, you have defined am optional _Option_ called `"EndDate"`. If this Option is not provided in the command-line the Property `DateTime EndDate` would be: `DateTime.DefautlValue`, and this is probably not what you wanted.

```csharp
[OptionDescriptor("EndDate", Required = false)]
public DateTime EndDate { get; set; }
```

You can catch this in the `BeforeExecute` method and check if any **Property** \(`Contains( nameof(ps.EndDate))` !!\) ****is unresolved - has not got a value from command-line - neither explicitly provided nor static default value:

```csharp
if (unresolvedPropertyNames.Contains(nameof(ps.EndDate)))
{
    // Set dynamic, dependent default value
    Console.WriteLine($"Unresolved {nameof(ps.EndDate)}");
    ps.EndDate = ps.StartDate.AddDays(7);
}
```

### Error Handling

Use the `errors.AddError()` method to _collect_  errors message. `Commander` will check for any error before it executes the Command and terminates the application gracefully, by printing out all error messages you collected. Collecting error messages is much more convenient the throwing _Exceptions_, because an Exception normally stop after the first error occurred. 

