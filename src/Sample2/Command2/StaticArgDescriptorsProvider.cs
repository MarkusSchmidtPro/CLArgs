using System.Collections.Generic;



namespace MSPro.CLArgs.Sample2.Command2
{
    internal class CommandArgsDescriptors : ArgsDescriptors
    {
        public CommandArgsDescriptors()
        {
            this.OptionDescriptors = new List<OptionDescriptor>
            {
                //
                // Descriptor for command-line option: 'opt1'
                //  uses constructor initialization
                //
                new OptionDescriptor(new[] {"--opt1", "-o"},
                    "Desc 1", "false", true),

                //
                // Descriptor for command-line option: 'opt2'
                //
                new OptionDescriptor
                {
                    Tags = new[] {"--opt1", "-o"},
                    Description = "Desc 2",
                    Default = "false",
                    Mandatory = true
                },
            };

            // no verbs supported
            this.VerbDescriptors = null;
        }
    }
}