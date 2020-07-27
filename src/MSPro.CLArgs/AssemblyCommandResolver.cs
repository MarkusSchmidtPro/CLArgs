using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace MSPro.CLArgs
{
    public class AssemblyCommandResolver : ICommandResolver
    {
        private readonly List<Assembly> _assemblies= new List<Assembly>();



        public AssemblyCommandResolver( IEnumerable<string> assemblyFileNames)
        {
            _assemblies = assemblyFileNames.Select(Assembly.LoadFile).ToList();
        }
        public AssemblyCommandResolver(Assembly assembly) 
        {
            _assemblies.Add(assembly);
        }

        public Dictionary<string, Type> GetCommandTypes()
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
            foreach (Assembly assembly in _assemblies)
            {
                GetTypesFromAssembly(assembly, dictionary);
            }
            return dictionary;
        }



        private static void GetTypesFromAssembly(Assembly assembly, IDictionary<string, Type> dictionary)
        {
            foreach (TypeInfo definedType in assembly.DefinedTypes)
            {
                CommandAttribute customAttribute = definedType.GetCustomAttribute<CommandAttribute>();
                if (customAttribute == null) continue;
                    
                if (definedType.ImplementedInterfaces.All(i => i != typeof(ICommand)))
                    throw new ApplicationException(
                        "Command " + customAttribute.Verb + " doe not implement the ICommand interface.");
                dictionary[customAttribute.Verb] = definedType;
            }
        }
    }
}