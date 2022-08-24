using MSPro.CLArgs;



/// <summary>
/// This class is used as a Context property, that is used by many other Contexts
/// </summary>
public class ConnectionParameters
{
    [OptionDescriptor("Username", required:true)]
    public string Username { get; set; }

    [OptionDescriptor("Password", required:true)]
    public string Password { get; set; }
}