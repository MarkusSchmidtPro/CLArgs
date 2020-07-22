using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace MSPro.CLArgs
{
    internal class OptionDescriptorFromTypeProvider<TCommandOptions>
    {
        public IEnumerable<OptionDescriptorAttribute> Get()
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