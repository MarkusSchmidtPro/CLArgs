using MSPro.CLArgs;



public class Mult23Context
{
    [OptionDescriptor("Factor1", new []{"f1", "a"},
                      required:true, defaultValue:0, helpText : "The first factor." )]
    public decimal Factor1 { get; set; }
    
    
    [OptionDescriptor("Factor2", new []{"f2","b"},
                      required:true, defaultValue:0, helpText : "The second factor." )]
    public decimal Factor2 { get; set; }
}