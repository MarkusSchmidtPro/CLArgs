using MSPro.CLArgs;



public class ApiContext : SharedContext
{
    [OptionDescriptor("P1", required:false, defaultValue :"my default", helpText:"Specify something.")]
    public string P1 { get; set; }

    [OptionSet]
    public ConnectionParameters ConnectionParameters{ get; set; }
}