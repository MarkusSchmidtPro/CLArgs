using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property)]
    [PublicAPI]
    public class OptionDescriptorAttribute : Attribute
    {
        public OptionDescriptorAttribute(string optionName, 
                                         string[] tags,
                                         bool required = false, 
                                         object defaultValue = null,
                                         string description = null)
        {
            this.OptionName = optionName;
            this.Tags        = tags;
            this.Description = description;
            this.Default     = defaultValue;
            this.Required    = required;
        }
        
        public OptionDescriptorAttribute(string optionName, 
                                         string tag=null,
                                         bool required = false, 
                                         object defaultValue = null,
                                         string description = null)
            :this(optionName,new[] {tag ?? optionName}, required, defaultValue, description)
        {
        }

        
        
        public string OptionName { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public object Default { get; set; }
        public bool Required { get; set; }



        public new string ToString() =>
            $"{this.OptionName}: [{string.Join(",", this.Tags)}], required={this.Required}, Default={this.Default}";
    }
}