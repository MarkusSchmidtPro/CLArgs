using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class StringConverter : ITypeConverter
    {
        public object FromString(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(string))
                throw new ArgumentException(
                    $"Cannot use {this.GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));
            
            return optionValue;
        }
    }
}