using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MSPro.CLArgs;



namespace Demo04;

/*
 * A demo how to build and use custom option types
 */



[Command("ARGS.DEMO4", "Demo how to build and use custom option types.")]
public class Demo1Command(IServiceProvider serviceProvider) : CommandBase2<Demo4Context>(serviceProvider)
{
    protected override void Execute()
    {
        Console.WriteLine($"Directory full-name: {_context.Dir!.FullName}");
        Console.WriteLine($"Directory parent: {_context.Dir!.Parent}");
    }


    protected override void BeforeExecute( ErrorDetailList errors)
    {
        var notProvidedProperties = ContextProperties.Where(cp=> !cp.IsProvided).ToList();
        if (notProvidedProperties.Count > 0)
        {
            Console.WriteLine($"{notProvidedProperties.Count} properties which were not provided in the command-line.");
            foreach (var contextProperty in notProvidedProperties)
            {
                string defaultText = contextProperty.IsDefault ? " [Default]" : "";
                var firstValue = contextProperty.HasValue ? contextProperty.ProvidedValues[0] : "null";
                Console.WriteLine($"{contextProperty.OptionName}={firstValue}{defaultText}");
            }
        }
        
        base.BeforeExecute( errors);
    }
}



public class Demo4Context
{
    /* There is no built-in converter to convert command-line arguments into
       a property of type DirectoryInfo, and we need to create and register (see program.cs)
       a custom converter,
       */
    [OptionDescriptor("Directory", new[] { "Dir", "d" },
                      true,  helpText: "Specify an existing directory.")]
    public DirectoryInfo? Dir { get; set; }


    /* Demo a custom validator */
    [OptionDescriptor("ThreeChars", new[] { "tc" },
                      false,  helpText:"Specify three capital characters.")]
    public string? ThreeChars { get; set; } 

    [OptionDescriptor("OptionalArgument", new[] { "o" }, 
                        defaultValue : "my default",
                        helpText:"You may provide this or leave it out.")]
    public string? Optional { get; set; }
}




/// <summary>
///     Custom argument to DirectoryInfo converter.
/// </summary>
public class DirectoryInfoConverter : IArgumentConverter
{
    public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType)
    {
        if (!Directory.Exists(optionValue))
            errors.AddError(optionName, $"The specified directory does not exist: '{optionValue}'.");

        return new DirectoryInfo(optionValue);
    }
}