using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class DateTimeConverter : ITypeConverter
    {
        public object FromString(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(DateTime))
                throw new ArgumentException(
                    $"Cannot use {this.GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));
            
            if (!DateTime.TryParse(optionValue, out DateTime d))
                errors.AddError(optionName, 
                                $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a DateTime.");
            return d;
        }
    }
}