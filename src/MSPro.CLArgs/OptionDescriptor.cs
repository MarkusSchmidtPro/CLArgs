using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace MSPro.CLArgs
{
    public class ArgsDescriptors
    {
        public ArgsDescriptors(IEnumerable<CommandLineOption> optionDescriptors)
        {
            OptionDescriptors = optionDescriptors.ToList();
        }
        public List<CommandLineOption> OptionDescriptors { get; set; }
        public List<VerbDescriptor> VerbDescriptors { get; set; }
    }
}