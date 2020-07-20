using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Represents a type converter to convert Option values (string)
    ///     to a target type.
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        ///     Convert from <c>string</c> int a
        ///     <paramref name="targetType" />
        ///     .
        /// </summary>
        /// <param name="value">The option value of type <c>string</c></param>
        /// <param name="propertyName">
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
        object FromString(string value, string propertyName, ErrorDetailList errors, Type targetType);
    }
}