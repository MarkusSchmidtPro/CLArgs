using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    public class CommandDescriptor
    {
        public CommandDescriptor([NotNull] string verb, [NotNull] Func<ICommand> factoryFunc, string description = null)
        {
            this.Verb                  = verb;
            this.CreateCommandInstance = factoryFunc;
            this.Description           = description;
        }

        public string Verb { get; }
        public Func<ICommand> CreateCommandInstance { get; }
        public string Description { get; }
    }
}