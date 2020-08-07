using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace MSPro.CLArgs
{
    public class AssemblyCommandResolver : ICommandResolver
    {
        private readonly List<Assembly> _assemblies = new List<Assembly>();



        public AssemblyCommandResolver(IEnumerable<string> assemblyFileNames)
        {
            _assemblies = assemblyFileNames.Select(Assembly.LoadFile).ToList();
        }



        public AssemblyCommandResolver(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }



        public List<CommandDescriptor> GetCommandDescriptors()
        {
            Dictionary<string, CommandDescriptor> dictionary = new Dictionary<string, CommandDescriptor>();
            foreach (Assembly assembly in _assemblies)
            {
                GetTypesFromAssembly(assembly, dictionary);
            }
            return dictionary.Values.ToList();
        }



        private static void GetTypesFromAssembly( Assembly assembly, IDictionary<string, CommandDescriptor> dictionary)
        {
            foreach (TypeInfo definedType in assembly.DefinedTypes)
            {
                CommandAttribute commandAttribute = definedType.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute == null) continue;

                if (definedType.ImplementedInterfaces.All(i => i != typeof(ICommand)))
                    throw new ApplicationException(
                        "Command " + commandAttribute.Verb + " doe not implement the ICommand interface.");
                dictionary[commandAttribute.Verb] = 
                    new CommandDescriptor(commandAttribute.Verb, 
                                          ()=> (ICommand) Activator.CreateInstance(definedType),
                                          commandAttribute.HelpText); 
            }
        }
    }
}