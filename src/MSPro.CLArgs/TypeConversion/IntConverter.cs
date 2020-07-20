using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class IntConverter : ITypeConverter
    {
        public object FromString(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(int))
                throw new ArgumentException(
                    $"Cannot use {this.GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));
            
            if (!int.TryParse(optionValue, out int v))
                errors.AddError(optionName,
                                     $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an integer.");
            return v;
        }
    }
}