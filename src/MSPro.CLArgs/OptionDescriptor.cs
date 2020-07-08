using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace MSPro.CLArgs
{
    public class ArgsDescriptors
    {
        public ArgsDescriptors(IEnumerable<CommandLineOptionAttribute> optionDescriptors)
        {
            OptionDescriptors = optionDescriptors.ToList();
        }
        public List<CommandLineOptionAttribute> OptionDescriptors { get; set; }
        public List<VerbDescriptor> VerbDescriptors { get; set; }
    }
}