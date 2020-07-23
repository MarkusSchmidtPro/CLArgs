using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    [PublicAPIAttribute]
    public class TypeConverters
    {
        /// <summary>
        ///     Convert from <c>string</c> int a
        ///     <paramref name="targetType" />
        ///     .
        /// </summary>
        /// <param name="value">The option value of type <c>string</c></param>
        /// <param name="optionName">
        ///     The name of the property on which the converter value will be set.
        /// </param>
        /// <param name="errors">
        ///     The error collection where to add conversion errors.
        /// </param>
        /// <param name="targetType">The into which <paramref name="value" /> should be converted.</param>
        /// <returns>
        ///     The converted value of type
        ///     <param ref="targetType"></param>
        /// </returns>
        /// <example>
        ///     <code>
        ///  if (!DateTime.TryParse(optionValue, out DateTime d)) {
        ///      errors.AddError(optionName,
        ///          $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a DateTime.");
        ///  }
        ///  return d;
        /// </code>
        /// </example>
        public delegate object FromStringDelegate(
            string value, string optionName, ErrorDetailList errors = null, Type targetType = null);



        public TypeConverters()
        {
            this._items = new Dictionary<Type, FromStringDelegate>
            {
                {typeof(DateTime), toDateTime},
                {typeof(string), toString},
                {typeof(int), toInt},
                {typeof(bool), toBool},
                {typeof(Enum), toEnum}
            };
        }



        private readonly Dictionary<Type, FromStringDelegate> _items;



        public object Convert(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            FromStringDelegate converter
                = _items.ContainsKey(targetType) ? _items[targetType] : _items[targetType.BaseType];
            return converter(optionName, optionValue, errors, targetType);
        }



        /// <summary>
        /// Register a converter that does not report conversion errors.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="convertFunc"></param>
        public void Register(Type targetType, Func<string, string, object> convertFunc)
        {
            // ReSharper disable once ConvertToLocalFunction
            FromStringDelegate convertWithOutErrors
                = (optionName, optionValue, errors, type) => convertFunc(optionName, optionValue);
            Register(targetType, convertWithOutErrors);
        }



        public void Register(Type targetType, FromStringDelegate convertFunc)
        {
            _items[targetType] = convertFunc;
        }



        public bool CanConvert(Type targetType) => 
            this._items.ContainsKey(targetType) || _items.ContainsKey(targetType.BaseType);



        #region Out-Of-The_Box Converters

        private object toString(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(string))
                throw new ArgumentException(
                    $"Cannot use {GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));

            return optionValue;
        }



        private object toInt(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(int))
                throw new ArgumentException(
                    $"Cannot use {GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));

            if (!int.TryParse(optionValue, out int v))
                errors.AddError(optionName,
                                $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an integer.");
            return v;
        }



        private object toBool(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(bool))
                throw new ArgumentException(
                    $"Cannot use {GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
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



        private object toDateTime(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
        {
            if (targetType != typeof(DateTime))
                throw new ArgumentException(
                    $"Cannot use {GetType()} to convert a string into {targetType}. OptionName={optionName}, OptionValue={optionValue}",
                    nameof(targetType));

            if (!DateTime.TryParse(optionValue, out DateTime d))
                errors.AddError(optionName,
                                $"Cannot parse the value '{optionValue}' for Option '{optionName}' into a DateTime.");
            return d;
        }



        private object toEnum(string optionName, string optionValue, ErrorDetailList errors, Type targetType)
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

        #endregion
    }
}