using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class BooleanConverter : ITypeConverter
    {
        public object FromString(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(bool))
                throw new ArgumentException(
                    $"Cannot use {this.GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));
            
            if (!bool.TryParse(optionValue, out var boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(optionValue, out var intValue))
                    // int conversion possible 
                    boolValue = intValue != 0;
                else
                    errors.AddError(optionName,
                                         $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an boolean.");
            }

            return boolValue;
        }
    }
}