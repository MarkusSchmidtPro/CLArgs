using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionDescriptorAttribute : Attribute
    {
        public OptionDescriptorAttribute(string optionName, string tag = null)
        {
            this.OptionName = optionName;
            this.Tags       = new[] {tag ?? optionName};
            this.Required   = false;
        }



        public string OptionName { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public object Default { get; set; }
        public bool Required { get; set; }



        public new string ToString() =>
            $"{this.OptionName}: [{DebuggerDisplayTags()}], required={this.Required}, Default={this.Default}";



#if DEBUG
        private string DebuggerDisplayTags() => $"{string.Join(",", this.Tags)}";
#endif
    }
}