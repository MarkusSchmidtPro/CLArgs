using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;



namespace MSPro.CLArgs
{
    internal class OptionDescriptorFromTypeProvider<TCommandOptions>
    {
        public IEnumerable<OptionDescriptorAttribute> Get() => getDescriptors(typeof(TCommandOptions));

        // if property is of type class
        // recursively dive down to check if there are more properties.

        private IEnumerable<OptionDescriptorAttribute> getDescriptors(Type t)
        {
            List<OptionDescriptorAttribute> result= new List<OptionDescriptorAttribute>();
            foreach (var pi in t.GetProperties())
            {
                if (getFirst<OptionSetAttribute>(pi) != null)
                {
                    result.AddRange( getDescriptors(pi.PropertyType));
                }
                else
                {
                    var optionDescriptor = getFirst<OptionDescriptorAttribute>(pi);
                    if( optionDescriptor!= null) result.Add(optionDescriptor);
                } 
            }
            return result;
        }



        private TAttribute getFirst<TAttribute>(ICustomAttributeProvider pi) where TAttribute : Attribute
        {
            var customAttributeOfType = pi.GetCustomAttributes(typeof(TAttribute), true);
            if (customAttributeOfType.Length == 0) return null;
            
            Debug.Assert(customAttributeOfType.Length == 1);
            return (TAttribute) customAttributeOfType[0];
        }
    }
}