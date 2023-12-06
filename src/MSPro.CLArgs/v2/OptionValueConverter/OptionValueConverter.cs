using System;

namespace MSPro.CLArgs
{
    public class StringConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType) =>
            optionValue;
    }

    public class IntConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType)
        {
            if (!int.TryParse(optionValue, out int v))
                errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an integer.");
            return v;
        }
    }

    public class DecimalConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType)
        {
            if (!decimal.TryParse(optionValue, out decimal v))
                errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a decimal.");
            return v;
        }
    }

    public class BoolConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetTypes)
        {
            if (bool.TryParse(optionValue, out bool boolValue)) return boolValue;

            // boolean conversion failed, try int conversion on <>0
            if (int.TryParse(optionValue, out int intValue))
                // int conversion possible 
                boolValue = intValue != 0;
            else
                errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an boolean.");

            return boolValue;
        }
    }

    public class DateTimeConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType)
        {
            if (!DateTime.TryParse(optionValue, out var d))
                errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a DateTime.");
            return d;
        }
    }

    public class EnumConverter : IArgumentConverter
    {
        public object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(Enum) && targetType.BaseType != typeof(Enum))
                throw new ArgumentException(
                    $"Cannot use {GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));

            if (!Enum.IsDefined(targetType, optionValue))
                errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a DateTime.");

            return Enum.Parse(targetType, optionValue, false);
        }
    }

    public interface IArgumentConverter
    {
        object Convert(string optionValue, string optionName, ErrorDetailList errors, Type targetType = null);
    }
}