using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MSPro.CLArgs;

/// <summary>
///     Provides functionality to resolve annotated ICommand implementations
///     in one or in a list of assemblies.
/// </summary>
public class AssemblyCommandResolver2 
{
    private readonly List<Assembly> _assemblies = new();


    public AssemblyCommandResolver2(IEnumerable<string> assemblyFileNames)
    {
        _assemblies = assemblyFileNames.Select(Assembly.LoadFile).ToList();
        _assemblies.Add(Assembly.GetExecutingAssembly());
    }

    public AssemblyCommandResolver2(Assembly assembly)
    {
        _assemblies.Add(assembly);
    }

    public List<CommandDescriptor2> GetCommandDescriptors()
    {
        Dictionary<string, CommandDescriptor2> dictionary = new();
        foreach (var assembly in _assemblies) getTypesFromAssembly(assembly, dictionary);
        return dictionary.Values.ToList();
    }


    private static void getTypesFromAssembly(
        Assembly assembly, IDictionary<string, CommandDescriptor2> dictionary)
    {
        foreach (var definedType in assembly.DefinedTypes)
        {
            var commandAttribute = definedType.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute == null) continue;

            if (definedType.ImplementedInterfaces.All(i => i != typeof(ICommand2)))
                throw new ApplicationException(
                    "Command " + commandAttribute.Verb + " does not implement the ICommand interface.");
            
            dictionary[commandAttribute.Verb] =
                new CommandDescriptor2(commandAttribute.Verb, definedType, commandAttribute.HelpText);
        }
    }
}