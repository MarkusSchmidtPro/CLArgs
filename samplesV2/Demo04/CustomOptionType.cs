using System.Text.RegularExpressions;
using MSPro.CLArgs;



namespace Demo04;

/*
 * A demo how to build and use custom option types
 */



[Command("ARGS.DEMO4", "Demo how to build and use custom option types.")]
public class Demo1Command : CommandBase2<Demo4Context>
{
    public Demo1Command(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }



    protected override void Execute()
    {
        Console.WriteLine($"Directory full-name: {this.Context.Dir!.FullName}");
        Console.WriteLine($"Directory parent: {this.Context.Dir!.Parent}");
    }



    /*protected override void BeforeExecute(HashSet<string> unresolvedPropertyNames, ErrorDetailList errors)
    {
        //Context.Validate(CommandOptions);
        base.BeforeExecute(unresolvedPropertyNames, errors);
    }*/
}



public class Demo4Context
{
    /*public void Validate(IOptionCollection options)
    {
        if( options..Length!=3) 
            errors.AddError(nameof(Context.ThreeChars), "Length must be exactly three characters!");
        
        var rx = new Regex("[A-Z]{3}");
        if( !rx.IsMatch(Context.ThreeChars!))
            errors.AddError(nameof(Context.ThreeChars), "Three characters are not three capital characters!");
    }*/
    
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