using System;



namespace MSPro.CLArgs
{
    internal class CommandWrapper : ICommand
    {
        private readonly Action<Arguments> _func;
        public CommandWrapper(Action<Arguments> func) => _func = func;
        public void Execute(Arguments commandParameters, Settings s) => _func(commandParameters);
    }
}