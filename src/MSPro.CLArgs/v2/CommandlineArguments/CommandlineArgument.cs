using System.Diagnostics;

namespace MSPro.CLArgs;

/// <summary>
///     A command-line Option.
/// </summary>
[DebuggerDisplay("{Type}({Key})='{Value}'")]
public class CommandlineArgument
{
    private CommandlineArgument(CommandlineArgumentType type, string tag, string value)
    {
        Type = type;
        Value = value;
        Key = tag;
    }

    public CommandlineArgumentType Type { get; }

    public string Key { get; }

    public string Value { get; set; }

    public static CommandlineArgument Option(string tag, string value) =>
        new(CommandlineArgumentType.Option, tag, value);

    public static CommandlineArgument Option(string tag, bool value = true) =>
        new(CommandlineArgumentType.Option, tag, value.ToString());

    public static CommandlineArgument Verb(string name) => new(CommandlineArgumentType.Verb, name, null);
    public static CommandlineArgument Target(string value) => new(CommandlineArgumentType.Target, null, value);
}