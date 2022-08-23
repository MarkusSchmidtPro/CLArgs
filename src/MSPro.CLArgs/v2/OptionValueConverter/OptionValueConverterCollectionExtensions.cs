using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[PublicAPI]
public static class OptionValueConverterCollectionExtensions
{
    public static IOptionValueConverterCollection AddCustomConverter(
        this IOptionValueConverterCollection optionValueConverters, 
        Type returnType, IArgumentConverter converter)
    {
        optionValueConverters.Add( returnType, converter );
        return optionValueConverters;
    }
}