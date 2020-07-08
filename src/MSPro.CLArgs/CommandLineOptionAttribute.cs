using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineOptionAttribute : Attribute
    {
        public CommandLineOptionAttribute(string name, string tag=null, string description=null, 
            string defaultValue=null, 
            bool mandatory=false) :
            this(tag, new[] {tag}, description, defaultValue, mandatory)
        {
            if (tag == null) tag = $"--{name}";
        }



        public CommandLineOptionAttribute(string name, string[] tags, string description=null, 
            string defaultValue=null, 
            bool mandatory=false)
        {
            this.Name = name;
            this.Tags = tags;
            this.Description = description;
            this.Default = defaultValue;
            this.Mandatory = mandatory;
        }



        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Default { get; set; }
        public bool Mandatory { get; set; }
    }
}