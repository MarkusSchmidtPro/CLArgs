using System.Diagnostics;



namespace MSPro.CLArgs;

/// <summary>
///     A single command-line argument.
/// </summary>
[DebuggerDisplay("{Type}({Key})='{Value}'")]
public class Argument
{
    private Argument(ArgumentType type, string tag, string value)
    {
        this.Type = type;
        this.Value = value;
        this.Key = tag;
    }



    public ArgumentType Type { get; }

    public string Key { get; }

    public string Value { get; set; }

    public static Argument Option(string tag, string value) => new(ArgumentType.Option, tag, value);
    public static Argument Verb(string name) => new(ArgumentType.Verb, name, null);
    public static Argument Target(string value) => new(ArgumentType.Target, value, value);
}