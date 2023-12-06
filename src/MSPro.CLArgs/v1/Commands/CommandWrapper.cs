using System;
using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public class CommandWrapper : ICommand
    {
        private readonly Action<CommandLineArguments> _func;
        public CommandWrapper(Action<CommandLineArguments> func) => _func = func;
        public List<OptionDescriptorAttribute> OptionDescriptors  => throw new NotImplementedException(); 
        public void Execute(CommandLineArguments commandParameters, Settings s) => _func(commandParameters);
    }
}