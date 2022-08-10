using System.Collections.Generic;

#pragma warning disable CS1591


namespace MSPro.CLArgs;

public interface ICommandResolver
{
    List<CommandDescriptor> GetCommandDescriptors();
}