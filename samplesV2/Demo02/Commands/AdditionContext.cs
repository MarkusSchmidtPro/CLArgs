using MSPro.CLArgs;



namespace CLArgs.Demo02;

public class AdditionContext
{
    [OptionDescriptor("Value1", new []{"a1", "a"},
                      required:true, defaultValue:0, helpText : "The first addend or the minuend." )]
    public int Value1 { get; set; }
    
    [OptionDescriptor("Value2", new []{"a2","b"},
                      required:true, defaultValue:0, helpText : "The second addend or the subtrahend." )]
    public int Value2 { get; set; }
}