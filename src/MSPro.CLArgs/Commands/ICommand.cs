using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public interface ICommand
    {
        List<OptionDescriptorAttribute> OptionDescriptors { get; }
        void Execute(CommandLineArguments commandParameters, Settings settings);
    }
}