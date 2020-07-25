using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace MSPro.CLArgs
{
    public class AssemblyCommandResolver : ICommandResolver
    {
        private readonly Assembly _assembly;



        public AssemblyCommandResolver(Assembly assembly) => _assembly = assembly;


        public Dictionary<string, Type> GetCommandTypes()
        {
            var result = new Dictionary<string, Type>();
            foreach (TypeInfo commandTypeInfo in _assembly.DefinedTypes)
            {
                var commandAttribute = commandTypeInfo.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute == null) continue;

                if (commandTypeInfo.ImplementedInterfaces.All(i => i != typeof(ICommand)))
                    throw new ApplicationException(
                        $"Command {commandAttribute.Verb} doe not implement the {nameof(ICommand)} interface.");

                result[commandAttribute.Verb] = commandTypeInfo; 
            }
            return result;
        }
    }
}