using System;
using System.Collections.Generic;
using MSPro.CLArgs.ErrorHandling;



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



        public TCommandOptions ToObject<TCommandOptions>(CommandLineOptions commandLineOptions) where TCommandOptions : class, new()
        {
            var result = new TCommandOptions();

            // 
            // Iterate through all properties of the target type
            //
            foreach (var pi in typeof(TCommandOptions).GetProperties())
            {
                // Need to find the related option-name for each property
                // 1. Check if property has a CommandLineOptionAttribute,
                //      then the attribute's Name specifies the option's name.
                // 2. Otherwise, the property-name will be used to look for a matching optionName.

                var customAttributeOfType = pi.GetCustomAttributes(typeof(CommandLineOptionAttribute), true);
                CommandLineOptionAttribute firstCommandLineOptionAttribute
                    = (CommandLineOptionAttribute) (customAttributeOfType.Length > 0 ? customAttributeOfType[0] : null);

                string optionName = firstCommandLineOptionAttribute != null ? firstCommandLineOptionAttribute.Name : pi.Name;
                // Provided or default value
                string optionValue = commandLineOptions.GetProvidedValue(optionName);
                if (optionValue == null)
                {
                    this.Errors.AddError(optionName,
                        $"Option {optionName} is missing from command-line and/or nor default value is specified." +
                        $"Cannot satisfy mapping for target property {pi.DeclaringType.Name}.{optionName}.");
                    continue;
                }

                if (!this.Converters.ContainsKey(pi.PropertyType))
                {
                    this.Errors.AddError(optionName,
                        $"No mapper found for type {pi.PropertyType} of property {pi.Name} ");
                    continue;
                }


                var mapFunc = this.Converters[pi.PropertyType];
                pi.SetValue(result, mapFunc(optionName, optionValue));
            }

            return result;
        }



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