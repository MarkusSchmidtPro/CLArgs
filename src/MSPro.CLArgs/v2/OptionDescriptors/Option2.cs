using System.Collections.Generic;



namespace MSPro.CLArgs;

public class Option2
{
    public string OptionName { get; set; }
    public string[] Tags { get; set; }
    public string HelpText { get; set; }
    public object Default { get; set; }
    public bool Required { get; set; }
    public string AllowMultiple { get; set; }
    public string AllowMultipleSplit { get; set; }


    /// <summary>
    /// An Options can be provided multiple times.
    /// </summary>
    public List<string> Values { get; } = new();

    public bool HasValue => Values.Count > 0;    


    public override string ToString() =>
        $"{this.OptionName}: [{string.Join(",", this.Tags)}], required={this.Required}, Default={this.Default}";
}
