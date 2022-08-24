using System.Collections.Generic;



namespace MSPro.CLArgs;


/// <summary>
/// Represent a Context property with all its describing information. 
/// </summary>
public class ContextProperty
{
    public string OptionName { get; set; }
    public string[] Tags { get; set; }
    public string HelpText { get; set; }
    public object Default { get; set; }
    public bool Required { get; set; }
    public string AllowMultiple { get; set; }
    public string AllowMultipleSplit { get; set; }



    public void SetValue(string commandlineValue)
    {
        ProvidedValues.Add(commandlineValue);
        IsProvided = true;
    }
    
    /// <summary>
    /// The values provided in the command-line (<see cref="IsProvided"/>=true)
    /// or using the <see cref="Default"/>.
    /// </summary>
    /// <remarks>
    /// An Options can be provided multiple times.
    /// </remarks>
    public List<string> ProvidedValues { get; } = new();

    
    /// <summary>
    /// True if a value was provided in the command-line.
    /// </summary>
    public bool HasValue => this.ProvidedValues.Count > 0;    

    public bool IsProvided { get; private set; }
    public bool IsDefault { get; private set; }

    public override string ToString() =>
        $"{this.OptionName}: [{string.Join(",", this.Tags)}], required={this.Required}, Default={this.Default}";


    public void UseDefault()
    {
        this.ProvidedValues.Add(this.Default.ToString());
        IsDefault = true;
    }
}
