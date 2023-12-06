using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Extension methods to ADD descriptors.
    /// </summary>
    [PublicAPI]
    public static class CommandDescriptorCollectionExtensions
    {
        public static ICommandDescriptorCollection AddDescriptor(
            this ICommandDescriptorCollection commandDescriptors,
            CommandDescriptor2 descriptor)
        {
            commandDescriptors.Add(descriptor.Verb, descriptor);
            return commandDescriptors;
        }



        public static ICommandDescriptorCollection AddAssembly(
            this ICommandDescriptorCollection commandDescriptors,
            Type type)
        {
            getTypesFromAssembly(type.Assembly, commandDescriptors);
            return commandDescriptors;
        }    
    
        public static ICommandDescriptorCollection AddAssembly(
            this ICommandDescriptorCollection commandDescriptors,
            Assembly assembly)
        {
            getTypesFromAssembly(assembly, commandDescriptors);
            return commandDescriptors;
        }



        public static ICommandDescriptorCollection AddAssemblies(
            this ICommandDescriptorCollection commandDescriptors,
            IEnumerable<string> assemblyFileNames)
        {
            var assemblies = assemblyFileNames.Select(Assembly.LoadFile);
            foreach (var assembly in assemblies)
            {
                getTypesFromAssembly(assembly, commandDescriptors);
            }

            return commandDescriptors;
        }



        private static void getTypesFromAssembly(
            Assembly assembly, ICommandDescriptorCollection dictionary)
        {
            foreach (var definedType in assembly.DefinedTypes)
            {
                var commandAttribute = definedType.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute == null) continue;

                if (definedType.ImplementedInterfaces.All(i => i != typeof(ICommand2)))
                    throw new ApplicationException(
                        "Command " + commandAttribute.Verb + " does not implement the ICommand interface.");
                if (dictionary.TryGetValue(commandAttribute.Verb, out CommandDescriptor2 value))
                {
                    // The command attribute refers to a known verb. It it refers to the same type,
                    // then the same Assembly has already been scanned and
                    // I assume the assembly was added twice by accident:
                    //  ConfigureCommands(commands => {
                    //      commands.AddAssembly(typeof(Class2).Assembly);
                    //      commands.AddAssembly(typeof(Class1).Assembly);
                    var existingType = value.Type;
                    var currentType = definedType;
                    // Assuming the Assembly has been scanned.
                    if (existingType.FullName == currentType.FullName) break;

                    // It is a different Type that implements a known Verb
                    throw new IndexOutOfRangeException(
                        $"Multiple different implementations for verb {commandAttribute.Verb} found!");
                }


                dictionary.Add(commandAttribute.Verb,
                    new CommandDescriptor2(commandAttribute.Verb, definedType, commandAttribute.HelpText));
            }
        }
    }
}