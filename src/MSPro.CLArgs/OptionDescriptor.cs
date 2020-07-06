using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public class ArgsDescriptors
    {
        public List<OptionDescriptor> OptionDescriptors { get; set; }
        public List<VerbDescriptor> VerbDescriptors { get; set; }
    }

    public class OptionDescriptor
    {
        public OptionDescriptor()
        {
        }



        public OptionDescriptor(string[] tags, string description, string defaultValue, bool mandatory)
        {
            this.Tags = tags;
            this.Description = description;
            this.Default = defaultValue;
            this.Mandatory = mandatory;
        }



        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Default { get; set; }
        public bool Mandatory { get; set; }
    }
}