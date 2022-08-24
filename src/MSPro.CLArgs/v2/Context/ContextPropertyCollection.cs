using System;
using System.Collections.Generic;



namespace MSPro.CLArgs;

/// <summary>
///     Represents all known arguments of type Verb, Option or target.
/// </summary>
public class ContextPropertyCollection : List<ContextProperty>
{
    private ContextPropertyCollection()
    {
    }



    public static ContextPropertyCollection FromType<TContext>()
        => FromType(typeof(TContext));



    public static ContextPropertyCollection FromType(Type contextType)
    {
        var result = new ContextPropertyCollection();
        result.parseType(contextType);
        return result;
    }



    private void parseType(Type t)
    {
        foreach (var pi in t.GetProperties())
        {
            if (pi.GetFirst<OptionSetAttribute>() != null)
            {
                parseType(pi.PropertyType);
            }
            else
            {
                var optionDescriptorAttribute = pi.GetFirst<OptionDescriptorAttribute>();
                if (optionDescriptorAttribute != null)
                    Add(new ContextProperty
                    {
                        Default            = optionDescriptorAttribute.Default,
                        Required           = optionDescriptorAttribute.Required,
                        Tags               = optionDescriptorAttribute.Tags,
                        AllowMultiple      = optionDescriptorAttribute.AllowMultiple,
                        HelpText           = optionDescriptorAttribute.HelpText,
                        OptionName         = optionDescriptorAttribute.OptionName,
                        AllowMultipleSplit = optionDescriptorAttribute.AllowMultipleSplit
                    });
            }
        }
    }
}