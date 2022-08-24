using MSPro.CLArgs;



/// <summary>
/// This part of the Context is used by many other Contexts.
/// </summary>
public abstract class SharedContext
{
    [OptionDescriptor("Url", "u", required:true, helpText:"Specify the API Url.")]
    public string Url { get; set; }
}