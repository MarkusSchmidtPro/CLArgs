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
            string helpText = null)
        {
            this.OptionName = optionName;
            this.Tags = tags;
            this.HelpText = helpText;
            this.Default = defaultValue;
            this.Required = required;
        }

        public OptionDescriptorAttribute(string optionName,
            string tag = null,
            bool required = false,
            object defaultValue = null,
            string helpText = null)
            : this(optionName, new[] {tag ?? optionName}, required, defaultValue, helpText)
        {
        }

        // CommandLine compatibility
        public OptionDescriptorAttribute(char tag,
            string optionName,
            bool required = false,
            object defaultValue = null,
            string helpText = null)
            : this(optionName, new[] {tag.ToString()}, required, defaultValue, helpText)
        {
        }


        public string OptionName { get; set; }
        public string[] Tags { get; set; }
        public string HelpText { get; set; }
        public object Default { get; set; }
        public bool Required { get; set; }
        public string AllowMultiple { get; set; }


        public override string ToString() =>
            $"{this.OptionName}: [{string.Join(",", this.Tags)}], required={this.Required}, Default={this.Default}";
    }
}