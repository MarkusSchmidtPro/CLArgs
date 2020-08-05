using System;
using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public interface ICommandResolver
    {
        List<CommandDescriptor> GetCommandDescriptors();
    }
}