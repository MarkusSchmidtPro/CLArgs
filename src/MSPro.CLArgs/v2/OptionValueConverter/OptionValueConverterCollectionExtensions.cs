using System;



namespace MSPro.CLArgs
{
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
}