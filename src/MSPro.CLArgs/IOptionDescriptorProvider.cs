using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace MSPro.CLArgs
{
    /// <summary>
    /// Provide the functionality to get a list of option descriptors.
    /// </summary>
    public interface IOptionDescriptorProvider
    {
        IEnumerable<OptionDescriptorAttribute> Get();
    }



    class OptionDescriptorFromTypeProvider<TCommandOptions>:  IOptionDescriptorProvider
    {
        IEnumerable<OptionDescriptorAttribute> IOptionDescriptorProvider.Get()
        {
            Type t = typeof(TCommandOptions);
            foreach (var pi in t.GetProperties())
            {
                var customAttributeOfType = pi.GetCustomAttributes(typeof(OptionDescriptorAttribute), true);
                Debug.Assert(customAttributeOfType.Length == 1);
                yield return (OptionDescriptorAttribute) customAttributeOfType[0];
            }
        }
    }
}