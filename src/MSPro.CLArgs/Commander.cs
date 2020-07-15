using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs.Sample3
{
    public class Commander
    {
        private readonly Dictionary<string, Func<ICommand>> _commands;



        public Commander()
        {
            _commands = new Dictionary<string, Func<ICommand>>();
        }



        public Commander(Dictionary<string, Func<ICommand>> commands)
        {
            _commands = commands;
        }



        public void AddDefault(Func<ICommand> factory) => _commands.Add("", factory);
        public void AddCommand(string verb, Func<ICommand> factory) => _commands.Add(verb, factory);

        public ICommand GetInstance(string verb) => !_commands.ContainsKey(verb) ? null : _commands[verb]();
        public ICommand GetDefault() => _commands.Count == 0 ? null : _commands.First().Value();
    }
}