using JetBrains.Annotations;

namespace MSPro.CLArgs;

/// <summary>
/// Extension methods to ADD descriptors.
/// </summary>
[PublicAPI]
public static class CommandDescriptorCollectionExtensions
{
    public static ICommandDescriptorCollection AddDescriptor(this ICommandDescriptorCollection commandDescriptors,
        CommandDescriptor2 descriptor)
    {
        commandDescriptors.Add(descriptor.Verb, descriptor);
        return commandDescriptors;
    }

    public static ICommandDescriptorCollection AddAssemblies(this ICommandDescriptorCollection commandDescriptors,
        AssemblyCommandResolver2 resolver)
    {
        foreach (var descriptor in resolver.GetCommandDescriptors()) commandDescriptors.Add(descriptor.Verb, descriptor);
        return commandDescriptors;
    }
}