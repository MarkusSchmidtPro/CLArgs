using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public interface ICommand
    {
        List<OptionDescriptorAttribute> OptionDescriptors { get; }
        void Execute(Arguments commandParameters, Settings settings);
    }
}