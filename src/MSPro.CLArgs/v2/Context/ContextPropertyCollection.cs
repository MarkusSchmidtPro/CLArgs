using System;
using System.Collections.Generic;
using System.Reflection;



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
        foreach (PropertyInfo pi in t.GetProperties())
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
                    (optionDescriptorAttribute.OptionName,
                        optionDescriptorAttribute.Tags,
                        optionDescriptorAttribute.Required)
                    {
                        Default = optionDescriptorAttribute.Default,
                        AllowMultiple = optionDescriptorAttribute.AllowMultiple,
                        HelpText = optionDescriptorAttribute.HelpText,
                        AllowMultipleSplit = optionDescriptorAttribute.AllowMultipleSplit
                    });
            }
        }
    }
}