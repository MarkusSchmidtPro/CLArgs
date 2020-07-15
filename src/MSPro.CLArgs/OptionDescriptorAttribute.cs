using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionDescriptorAttribute : Attribute
    {
        public OptionDescriptorAttribute(string name, string tag = null)
            //, string tag = null, string description = null,
            //string defaultValue = null,
            //bool mandatory = false) :
            //this(tag, new[] {tag}, description, defaultValue, mandatory)
        {
            this.Name     = name;
            this.Tags     = new[] {tag ?? name};
            this.Required = false;
        }



        public OptionDescriptorAttribute(string name, string[] tags,
                                         bool required = false, object defaultValue = null,
                                         string description = null)
        {
            this.Name        = name;
            this.Tags        = tags;
            this.Description = description;
            this.Default     = defaultValue;
            this.Required    = required;
        }



        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public object Default { get; set; }
        public bool Required { get; set; }
    }
}