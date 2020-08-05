using System;
using System.Collections.Generic;



namespace MSPro.CLArgs
{
    internal class CommandWrapper : ICommand
    {
        private readonly Action<Arguments> _func;
        public CommandWrapper(Action<Arguments> func) => _func = func;
        public List<OptionDescriptorAttribute> OptionDescriptors  => throw new NotImplementedException(); 
        public void Execute(Arguments commandParameters, Settings s) => _func(commandParameters);
    }
}