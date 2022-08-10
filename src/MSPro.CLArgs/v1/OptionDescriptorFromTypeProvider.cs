using System;
using System.Collections.Generic;



namespace MSPro.CLArgs;

public class OptionDescriptorFromTypeProvider<TCommandContext>
{
    public IEnumerable<OptionDescriptorAttribute> Get() => getDescriptors(typeof(TCommandContext));

    // if property is of type class
    // recursively dive down to check if there are more properties.

    private IEnumerable<OptionDescriptorAttribute> getDescriptors(Type t)
    {
        List<OptionDescriptorAttribute> result= new();
        foreach (var pi in t.GetProperties())
        {
            if (pi.GetFirst<OptionSetAttribute>() != null)
            {
                result.AddRange( getDescriptors(pi.PropertyType));
            }
            else
            {
                var optionDescriptor = pi.GetFirst<OptionDescriptorAttribute>();
                if( optionDescriptor!= null) result.Add(optionDescriptor);
            } 
        }
        return result;
    }
}