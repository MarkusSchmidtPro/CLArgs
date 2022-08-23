using MSPro.CLArgs;



namespace SampleCommands.Math.Sum;

public class SumContext
{
    // The properties that AllowMultiple must be of type string.
    // While the Multiple property is of type IList<string>.
    
    [OptionDescriptor("Value", "v", 
                      helpText:"Specify any number of value to built a total sum.",
                      AllowMultiple = nameof(Values),
                      AllowMultipleSplit = ";,")]
    public string? Value { get; set; }

    
    public List<string> Values { get; set; } = new ();
}