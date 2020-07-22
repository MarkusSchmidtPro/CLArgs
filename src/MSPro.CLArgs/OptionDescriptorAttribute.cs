using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionDescriptorAttribute : Attribute
    {
        public OptionDescriptorAttribute(string optionName, string tag = null)
        {
            this.OptionName     = optionName;
            this.Tags     = new[] {tag ?? optionName};
            this.Required = false;
        }


        //
        // public OptionDescriptorAttribute(string optionName, string[] tags,
        //                                  bool required = false, object defaultValue = null,
        //                                  string description = null)
        // {
        //     this.OptionName        = optionName;
        //     this.Tags        = tags;
        //     this.Description = description;
        //     this.Default     = defaultValue;
        //     this.Required    = required;
        // }



        public string OptionName { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public object Default { get; set; }
        public bool Required { get; set; }
    }
}