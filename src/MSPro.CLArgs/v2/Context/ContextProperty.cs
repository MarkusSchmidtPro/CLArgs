using System.Collections.Generic;

namespace MSPro.CLArgs;

/// <summary>
/// Represent a Context property with all its describing information. 
/// </summary>
public class ContextProperty(string optionName, string[] tags, bool required)
{
    public string OptionName { get; } = optionName;
    public string[] Tags { get; } = tags;
    public bool Required  { get; set; } = required;

    public string? HelpText { get; set; }
    public object? Default { get; set; }
    public string? AllowMultiple { get; set; }
    public string? AllowMultipleSplit { get; set; }


    public void SetValue(string commandlineValue) 
        => ProvidedValues.Add(commandlineValue);


    public List<string> ProvidedValues { get; } = [];

    
    /// <summary>
    /// True if a value was provided in the command-line.
    /// </summary>
    public bool HasValue => ProvidedValues.Count > 0;


    public override string ToString() =>
        $"{OptionName}: [{string.Join(",", Tags)}], required={Required}, Default={Default}";


    public void UseDefault() 
        => ProvidedValues.Add(Default!.ToString()!);
}