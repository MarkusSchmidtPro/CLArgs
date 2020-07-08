﻿using System;
using System.Collections.Generic;
using MSPro.CLArgs.ErrorHandling;


// Default value as type
// Value resolver


namespace MSPro.CLArgs
{
    internal class CommandLineOptionsConverter
    {
        public CommandLineOptionsConverter()
        {
            this.Converters = new Dictionary<Type, Func<string, string, object>>
            {
                {typeof(string), toString},
                {typeof(int), toInt},
                {typeof(bool), toBool}
            };
        }



        private Dictionary<Type, Func<string, string, object>> Converters { get; }
        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        public TCommandOptions ToObject<TCommandOptions>(CommandLineOptions commandLineOptions, OptionResolverAction<TCommandOptions> optionResolver=null) where TCommandOptions : class, new()
        {
            var targetInstance = new TCommandOptions();
            List<string> unresolvedOptionNames= new List<string>();

            // 
            // Iterate through all properties of the target type
            //
            foreach (var targetPropertyInfo in typeof(TCommandOptions).GetProperties())
            {
                // Need to find the related option-name for each property
                // The option was created from the command-line.
                // 1. Check if current Target-Property has a CommandLineOptionAttribute,
                //      then the attribute's Name specifies the option's name.
                // 2. Otherwise, the Property-Name will be used to look for a matching optionName.

                var allCustomAttributesOfType = targetPropertyInfo.GetCustomAttributes(typeof(CommandLineOptionAttribute), true);
                CommandLineOptionAttribute firstCommandLineOptionAttribute
                    = (CommandLineOptionAttribute) (allCustomAttributesOfType.Length > 0 ? allCustomAttributesOfType[0] : null);

                string optionName = firstCommandLineOptionAttribute != null ? firstCommandLineOptionAttribute.Name : targetPropertyInfo.Name;
                // Provided or static default value
                string optionValue = commandLineOptions.GetProvidedValue(optionName);
                if (optionValue == null)
                {
                    //this.Errors.AddError(optionName,
                    //    $"Option {optionName} is missing from command-line and/or nor default value is specified." +
                    //    $"Cannot satisfy mapping for target property {targetPropertyInfo.DeclaringType.Name}.{optionName}.");
                    unresolvedOptionNames.Add(optionName);
                    continue;
                }

                if (!this.Converters.ContainsKey(targetPropertyInfo.PropertyType))
                {
                    this.Errors.AddError(optionName,
                        $"No mapper found for type {targetPropertyInfo.PropertyType} of property {targetPropertyInfo.Name} ");
                    continue;
                }


                var mapFunc = this.Converters[targetPropertyInfo.PropertyType];
                targetPropertyInfo.SetValue(targetInstance, mapFunc(optionName, optionValue));

                optionResolver?.Invoke(unresolvedOptionNames, targetInstance, Errors);
            }

            return targetInstance;
        }

        public delegate void OptionResolverAction<in TTarget>(IEnumerable<string> unresolvedOptionNames, TTarget targetInstance, ErrorDetailList errors);


        #region Default Converters

        private object toInt(string optionName, string optionValue)
        {
            if (!int.TryParse(optionValue, out var v))
                this.Errors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an integer.");
            return v;
        }



        private object toString(string optionName, string optionValue) => optionValue;



        private object toBool(string optionName, string optionValue)
        {
            if (!bool.TryParse(optionValue, out var boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(optionValue, out var intValue))
                    // int conversion possible 
                    boolValue = intValue != 0;
                else
                    this.Errors.AddError(optionName,
                        $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an boolean.");
            }

            return boolValue;
        }

        #endregion
    }
}