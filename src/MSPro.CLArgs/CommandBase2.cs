using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public abstract class CommandBase2<TCommandOptions> : ICommand2 where TCommandOptions : class, new()
    {
        private readonly Dictionary<Type, Func< string, string, object>> _argumentMapperFunctions;



        protected CommandBase2()
        {
            _argumentMapperFunctions = new Dictionary<Type, Func< string, string, object>>
            {
                {typeof(string), toString},
                {typeof(int), toInt},
                {typeof(bool), toBool}
            };
        }


        protected virtual TCommandOptions OnMap(CommandLineOptions commandLineOptions)
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
                    = (CommandLineOptionAttribute)(customAttributeOfType.Length > 0 ? customAttributeOfType[0] : null);

                string optionName = firstCommandLineOptionAttribute != null ? firstCommandLineOptionAttribute.Name : pi.Name;
                // Provided or default value
                string optionValue = commandLineOptions.GetProvidedValue(optionName);
                if (optionValue == null)
                {
                    this.ValidationErrors.AddError(optionName, $"Missing option {optionName}, unknown mapping for target property {pi.Name}.{optionName}.");
                    continue;
                }

                if (!_argumentMapperFunctions.ContainsKey(pi.PropertyType))
                {
                    this.ValidationErrors.AddError(optionName,
                        $"No mapper found for type {pi.PropertyType} of property {pi.Name} ");
                    continue;
                }


                var mapFunc = _argumentMapperFunctions[pi.PropertyType];
                pi.SetValue(result, mapFunc(optionName, optionValue));
            }

            return result;
        }



        protected abstract void OnExecute(TCommandOptions parameters);



        #region Default Mappers

        private object toInt(string optionName, string optionValue)
        {
            if (!int.TryParse(optionValue, out var v))
                this.ValidationErrors.AddError(optionName,
                    $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an integer.");
            return v;
        }



        private object toString( string optionName, string optionValue) 
            => optionValue ;



        private object toBool(string optionName, string optionValue)
        {
            if (!bool.TryParse(optionValue, out var boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(optionValue, out var intValue))
                    // int conversion possible 
                    boolValue = intValue != 0;
                else
                    this.ValidationErrors.AddError(optionName,
                        $"Cannot parse the value '{optionValue}' for Option '{optionName}' into an boolean.");
            }

            return boolValue;
        }

        #endregion



        #region ICommand2

        public ErrorDetailList ValidationErrors { get; } = new ErrorDetailList();



        void ICommand2.Execute(Arguments arguments, bool throwIf)
        {
            var options = CommandLineOptions.FromArguments<TCommandOptions>(arguments);
            if (options.Errors.HasErrors()) Debugger.Break();

            {
                var parameters = OnMap(options);
                if (!this.ValidationErrors.HasErrors()) OnExecute(parameters);
            }

            if (!this.ValidationErrors.HasErrors() || !throwIf) return;

            throw new AggregateException(this.ValidationErrors.Details.Select(
                e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }

        #endregion
    }
}