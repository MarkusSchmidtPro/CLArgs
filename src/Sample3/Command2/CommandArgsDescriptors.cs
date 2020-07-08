using System.Collections.Generic;



namespace MSPro.CLArgs.Sample3.Command2
{
    internal class CommandArgsDescriptors : ArgsDescriptors
    {
        public CommandArgsDescriptors() : base(new List<CommandLineOption>
        {
            //
            // Descriptor for command-line option: 'opt1'
            //  uses constructor initialization
            //
            new CommandLineOption("Option 1", new[] {"--opt1", "-o"},
                "Desc 1", "false", true),

            //
            // Descriptor for command-line option: 'opt2'
            //
            new CommandLineOption("Option 2")
            {
                Tags = new[] {"--opt1", "-o"},
                Description = "Desc 2",
                Default = "123",
                Mandatory = true
            },
        }){}
    }
}