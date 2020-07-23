using System;
using System.Collections.Generic;



namespace MSPro.CLArgs
{
    public interface ICommandResolver
    {
        Dictionary<string, Type> GetCommandTypes();
    }
}