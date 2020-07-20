using System;
using System.Collections.Generic;
using MSPro.CLArgs.ErrorHandling;



// Default value as type
// Value resolver



namespace MSPro.CLArgs
{
    internal class CommandLineOptionsConverter
    {
        
        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        /// <summary>
        ///     Convert the options provided in the command-line into an object.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type of the object that is created and populated.</typeparam>
        /// <param name="commandLineOptions">
        ///     The options and their values as provided in the command-line and those who had a
        ///     default value.
        /// </param>
        /// <param name="unresolvedProperties">
        ///     A list of properties which were not resolved - where there
        ///     was no value provided in the command-line. Only not <see cref="OptionDescriptorAttribute.Required" />
        ///     options will be listed here, because required options must be in the command-line or they are listed
        ///     in the <see cref="Errors" /> list.
        /// </param>
        /// <returns></returns>
        public TCommandOptions ToCommandParameters<TCommandOptions>(
            CommandLineOptions commandLineOptions, out List<string> unresolvedProperties)
            where TCommandOptions : class, new()
        {
            var targetInstance = new TCommandOptions();
            unresolvedProperties = new List<string>();

            // 
            // Iterate through all properties of the target type 
            //  which are decorated with a CommandLineOptionAttribute
            //
            foreach (var targetPropertyInfo in typeof(TCommandOptions).GetProperties())
            {
                // Need to find the related option-name for each property
                // The option was created from the command-line.
                // 1. Check if current Target-Property has a CommandLineOptionAttribute,
                //      then the attribute's Name specifies the option's name.
                // 2. Otherwise, the Property-Name will be used to look for a matching optionName.

                var allCustomAttributesOfType =
                    targetPropertyInfo.GetCustomAttributes(typeof(OptionDescriptorAttribute), true);
                OptionDescriptorAttribute firstOptionDescriptorAttribute
                    = (OptionDescriptorAttribute) (allCustomAttributesOfType.Length > 0
                        ? allCustomAttributesOfType[0]
                        : null);
                if (firstOptionDescriptorAttribute == null) continue;

                string optionName = firstOptionDescriptorAttribute.Name;
                string optionValue = commandLineOptions.GetProvidedValue(optionName);
                if (optionValue == null)
                {
                    //this.Errors.AddError(optionName,
                    //    $"Option {optionName} is missing from command-line and/or nor default value is specified." +
                    //    $"Cannot satisfy mapping for target property {targetPropertyInfo.DeclaringType.Name}.{optionName}.");
                    unresolvedProperties.Add(targetPropertyInfo.Name);
                    continue;
                }

                Type targetType = targetPropertyInfo.PropertyType;
                if (!TypeConverters.CanConvert(targetType))
                {
                    Errors.AddError(optionName,
                                    $"No type converter found for type {targetType} of property {optionName} ");
                }
                else
                {
                    object propertyValue =TypeConverters.Convert(optionName, optionValue, this.Errors, targetType);
                    targetPropertyInfo.SetValue(targetInstance, propertyValue);
                }
            }

            return targetInstance;
            
        }
    }
}