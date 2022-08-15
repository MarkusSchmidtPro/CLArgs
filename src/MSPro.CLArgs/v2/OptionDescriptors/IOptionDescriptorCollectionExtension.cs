using System;



namespace MSPro.CLArgs;

public static class IOptionDescriptorCollectionExtension
{
    public static IOptionCollection AddContextType<TContext>(this IOptionCollection optionCollection)
    {
         optionCollection.AddContextType(typeof(TContext));
         return optionCollection;
    }



    public static void AddContextType(this IOptionCollection optionCollection, Type contextType)
    {
        foreach (var pi in contextType.GetProperties())
        {
            if (pi.GetFirst<OptionSetAttribute>() != null)
            {
                optionCollection.AddContextType(pi.PropertyType);
            }
            else
            {
                var optionDescriptorAttribute = pi.GetFirst<OptionDescriptorAttribute>();
                if (optionDescriptorAttribute != null)
                    optionCollection.Add(new Option2
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