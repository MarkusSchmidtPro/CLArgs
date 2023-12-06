using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides functionality to resolve Commands in one or in a list of assemblies.
    /// </summary>
    public class AssemblyCommandResolver : ICommandResolver
    {
        private readonly List<Assembly> _assemblies = new();


        public AssemblyCommandResolver(IEnumerable<string> assemblyFileNames)
        {
            _assemblies = assemblyFileNames.Select(Assembly.LoadFile).ToList();
            _assemblies.Add(Assembly.GetExecutingAssembly());
        }

        public AssemblyCommandResolver(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        public List<CommandDescriptor> GetCommandDescriptors()
        {
            Dictionary<string, CommandDescriptor> dictionary = new();
            foreach (var assembly in _assemblies) getTypesFromAssembly(assembly, dictionary);
            return dictionary.Values.ToList();
        }


        private static void getTypesFromAssembly(
            Assembly assembly, IDictionary<string, CommandDescriptor> dictionary)
        {
            foreach (var definedType in assembly.DefinedTypes)
            {
                var commandAttribute = definedType.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute == null) continue;

                if (definedType.ImplementedInterfaces.All(i => i != typeof(ICommand)))
                    throw new ApplicationException(
                        "Command " + commandAttribute.Verb + " does not implement the ICommand interface.");
                dictionary[commandAttribute.Verb] =
                    new CommandDescriptor(commandAttribute.Verb,
                        () => (ICommand)Activator.CreateInstance(definedType),
                        commandAttribute.HelpText);
            }
        }
    }
}